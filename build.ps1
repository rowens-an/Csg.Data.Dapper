#!/usr/bin/env powershell
#requires -version 4
#
# CSG Build Script
# Copyright 2017 Cornerstone Solutions Group
Param(
	[alias("c")][string]
	$Configuration = "Release",
	[string]
	$BuildToolsVersion = "1.0-latest",
	[switch]
	$NoTest,
	[switch]
	$NoPackage,
	[string]
	$PullRequestNumber=""
)
. "$PSScriptRoot/bootstrap.ps1"	

$Solution =  "$(Get-Item -Path *.sln | Select-Object -First 1)"
$OutputPackages = @(
	".\Csg.Data.Dapper\Csg.Data.Dapper.csproj"
)
$TestProjects = @() #Get-Item -Path tests\**\*Tests.csproj | %{ $_.FullName }
$SkipPackage = $NoPackage.IsPresent

if ($PullRequestNumber) {
    Write-Host "Building for a pull request (#$PullRequestNumber), skipping packaging." -ForegroundColor Yellow
    $SkipPackage = $true
}

Write-Host "==============================================================================" -ForegroundColor DarkYellow
Write-Host "The Build Script for Csg.ListQuery"
Write-Host "==============================================================================" -ForegroundColor DarkYellow
Write-Host "Build Tools:`t$BuildToolsVersion"
Write-Host "Solution:`t$Solution"
Write-Host "Skip Tests:`t$NoTest"
Write-Host "Pull Req:`t$PullRequestNumber"
Write-Host "==============================================================================" -ForegroundColor DarkYellow

try {

	Get-BuildTools -Version $BuildToolsVersion | Out-Null
	
	# RESTORE
	Write-Host "Restoring Packages..." -ForegroundColor Magenta
	dotnet restore $SOLUTION
	if ($LASTEXITCODE -ne 0) {
		throw "Package restore failed with exit code $LASTEXITCODE."
	}

	# BUILD SOLUTION
	Write-Host "Performing build..." -ForegroundColor Magenta	
	dotnet build $SOLUTION --configuration $Configuration
	if ($LASTEXITCODE -ne 0) {
		throw "Build failed with exit code $LASTEXITCODE."
	}

	# RUN TESTS
	if ( !($NoTest.IsPresent) -and $TestProjects.Length -gt 0 ) {
		Write-Host "Performing tests..." -ForegroundColor Magenta
		foreach ($test_proj in $TestProjects) {
			Write-Host "Testing $test_proj"			
			#Note: The --logger parameter is for specifically for mstest to make it output test results
			dotnet test $test_proj --no-build --configuration $Configuration --logger "trx;logfilename=TEST-$(get-date -format yyyyMMddHHmmss).trx"
			if ($LASTEXITCODE -ne 0) {
				throw "Test failed with code $LASTEXITCODE"
			}
		}
	}

	# CREATE NUGET PACKAGES
	if ( !($SkipPackage) -and $OutputPackages.Length -gt 0 ) {
		Write-Host "Packaging..."  -ForegroundColor Magenta
		foreach ($pack_proj in $OutputPackages){
			Write-Host "Packing $pack_proj"
			dotnet pack $pack_proj --no-build --configuration $Configuration
			if ($LASTEXITCODE -ne 0) {
				throw "Pack failed with code $result"
			}
		}
	}

	Write-Host "All Done. This build is great! (as far as I can tell)" -ForegroundColor Green
	exit 0
} catch {
	Write-Host "ERROR: An error occurred and the build was aborted." -ForegroundColor White -BackgroundColor Red
	Write-Error $_	
	exit 3
} finally {
	Remove-Module 'BuildTools' -ErrorAction Ignore
}