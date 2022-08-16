#!/usr/bin/env powershell
#requires -version 4
#
# CSG Build Script
# Copyright 2017 Cornerstone Solutions Group
Param(
	[alias("c")][string]
	$Configuration = "Release",
	[string]
	$BuildToolsVersion = "1.1.0-latest",
	[switch]
	$NoTest,
	[switch]
	$NoPackage,
	[string]
	$PullRequestNumber=""
)

$Solution =  "$(Get-Item -Path *.sln | Select-Object -First 1)"
$OutputPackages = @(
	".\Csg.Data.Dapper\Csg.Data.Dapper.csproj"
)
$TestProjects = @() #Get-Item -Path tests\**\*UnitTest.csproj | %{ $_.FullName }
$SkipPackage = $NoPackage.IsPresent

if ($PullRequestNumber) {
    Write-Host "Building for a pull request (#$PullRequestNumber), skipping packaging." -ForegroundColor Yellow
    $SkipPackage = $true
}
Write-Host "==============================================================================" -ForegroundColor DarkYellow
Write-Host "The Build Script for Csg.Data.Dapper"
Write-Host "==============================================================================" -ForegroundColor DarkYellow
Write-Host "Build Tools:`t$BuildToolsVersion"
Write-Host "Solution:`t$Solution"
Write-Host "Skip Tests:`t$NoTest"
Write-Host "Pull Req:`t$PullRequestNumber"
Write-Host "==============================================================================" -ForegroundColor DarkYellow

try {
	. "$PSScriptRoot/bootstrap.ps1"
	Get-BuildTools -Version $BuildToolsVersion | Out-Null

	# Get msbuild exe reference so we can use that instead of dotnet build, since we have a legacy project type (sqlproj)
	$msbuild = Find-MSBuild

	# RESTORE
	Write-Host "Restoring Packages..." -ForegroundColor Magenta
	& $msbuild /t:Restore /v:m $SOLUTION
	if ($LASTEXITCODE -ne 0) {
		throw "Package restore failed with exit code $LASTEXITCODE."
	}

	# BUILD SOLUTION
	Write-Host "Performing build..." -ForegroundColor Magenta
	# we are using msbuild here because the dacpac (database) project is a legacy (non-SDK) project.
	& $msbuild /p:Configuration=$Configuration /v:m $SOLUTION
	if ($LASTEXITCODE -ne 0) {
		throw "Build failed with exit code $LASTEXITCODE."
	}

	# RUN TESTS
	if ( !($NoTest.IsPresent) -and $TestProjects.Length -gt 0 ) {
		Write-Host "Performing tests..." -ForegroundColor Magenta
		foreach ($test_proj in $TestProjects) {
			Write-Host "Testing $test_proj"
			dotnet test $test_proj --no-build --configuration $Configuration #--filter TestCategory=Unit
			if ($LASTEXITCODE -ne 0) {
				throw "Test failed with code $LASTEXITCODE"
			}
		}
	}

	# CREATE NUGET PACKAGES
	if ( $OutputPackages.Length -gt 0 ) {
		Write-Host "Packaging..."  -ForegroundColor Magenta
		foreach ($pack_proj in $OutputPackages){
			Write-Host "Packing $pack_proj"
			dotnet pack $pack_proj --no-build --configuration $Configuration
			if ($LASTEXITCODE -ne 0) {
				throw "Pack failed with code $result"
			}
		}
	}

	# Publish asp.net core projects
	Write-Host "Publishing..."  -ForegroundColor Magenta
	dotnet publish $PublishProject --framework net461 --no-build --no-restore --configuration $Configuration

	if ($LASTEXITCODE -ne 0) {
		throw "Publish failed with code $result"
	}

	Write-Host "All Done. Let the record show that this build worked. " -ForegroundColor Green
	exit 0
} catch {
	Write-Host "ERROR: An error occurred and the build was aborted." -ForegroundColor White -BackgroundColor Red
	Write-Error $_
	exit 3
} finally {
	Remove-Module 'BuildTools' -ErrorAction Ignore
}