@echo off
if "%~1"=="" GOTO NoVersion
@echo on
nuget push MicroMapper.%1.nupkg -Source https://www.nuget.org/api/v2/package
GOTO End

:NoVersion
    echo ERROR: You must supply a version to publish

:End