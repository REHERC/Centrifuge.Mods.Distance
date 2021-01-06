@ECHO OFF
PUSHD %~dp0
SETLOCAL EnableDelayedExpansion

SET PACKAGES=%CD%\Build

CALL :BUILD_APP "AdventureMaker" "Distance Adventure Maker - Editor"
CALL :BUILD_APP "CustomDeathMessages" "Distance Custom Death Messages - Editor"

GOTO :EOF

REM ===== BUILD SUBROUTINES  =====

:BUILD_APP
SET PROJECT=%2
SET PROJECT=%PROJECT:~1,-1%

SET OUTPUT=%1
SET OUTPUT=%OUTPUT:~1,-1%

REM BUILD
ECHO Building %PROJECT%...
PUSHD "App.%OUTPUT%.Windows
dotnet publish -p:PublishProfile=Properties\PublishProfiles\win-x86.pubxml -o:publish\win-x86
POPD
PUSHD "App.%OUTPUT%.Linux
dotnet publish -p:PublishProfile=Properties\PublishProfiles\linux-x64.pubxml -o:publish\linux-x64
POPD
REM PACKAGING
ECHO Packaging %PROJECT%...
SET TARGET=%PACKAGES%\%PROJECT%
ECHO Target: %TARGET%
CALL :MAKEDIR %PACKAGES%\%APPNAME%
CALL :COPYDIR "%CD%\App.%OUTPUT%.Windows\publish\win-x86" "%TARGET%\win-x86"
CALL :COPYDIR "%CD%\App.%OUTPUT%.Linux\publish\linux-x64" "%TARGET%\linux-x64"

GOTO :EOF

REM =====  UTIL SUBROUTINES  =====

:MAKEDIR
IF NOT EXIST %1 MKDIR %1
GOTO :EOF

:COPY
ECHO NUL > %2
XCOPY /s /Y /v %1 %2
GOTO :EOF

:COPYDIR
CALL :MAKEDIR %2
XCOPY /i /s /Y /v %1 %2
GOTO :EOF