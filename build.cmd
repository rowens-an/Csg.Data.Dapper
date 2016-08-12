@ECHO OFF
setlocal

SET SOLUTION=Csg.Data.Dapper.sln
SET BUILD_CONFIG=Release
SET EnableNuGetPackageRestore=True

set MSBuild="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
if not exist %MSBuild% @set MSBuild="%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
if not exist %MSBuild% @set MSBuild="%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

set MSTest="C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\MSTest.exe"
if not exist %MSTest% @set MSTest="C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\MSTest.exe"

IF NOT EXIST .\bin\	MKDIR .\bin\
IF EXIST .\bin\%BUILD_CONFIG% RMDIR /Q /S .\bin\%BUILD_CONFIG%

ECHO BUILDING...

dotnet restore

ECHO . > .\bin\msbuild-%BUILD_CONFIG%.log
%MSBuild% %SOLUTION% /p:Configuration=%BUILD_CONFIG% /v:M /flp:LogFile=.\bin\msbuild-%BUILD_CONFIG%.log;Verbosity=Normal
IF ERRORLEVEL 1 GOTO BuildFail

REM ECHO TESTING...
REM dotnet test .\Csg.Data.Dapper.Tests
REM IF ERRORLEVEL 1 GOTO TestFail

popd

GOTO End

:BuildFail
echo.
echo *** BUILD FAILED ***
EXIT /b 1

:TestFail
echo.
echo *** TESTS FAILED ***
EXIT /b 2

:BuildSuccess
echo.
echo *** BUILD SUCCESSFUL ***
goto End

:End
echo DONE