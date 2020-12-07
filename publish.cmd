@ECHO OFF
PUSHD %~dp0

REM ===== COMPILE =====
REM Custom Death Messages App
PUSHD App.CustomDeathMessages.Windows
dotnet publish -p:PublishProfile=win-x86 -o:publish\win-x86
dotnet publish -p:PublishProfile=win-x64 -o:publish\win-x64
POPD
PUSHD App.CustomDeathMessages.Linux
dotnet publish -p:PublishProfile=linux-x64 -o:publish\linux-x64
POPD

REM ===== BUNDLE =====
SET PACKAGES=%CD%\Build
CALL :MAKEDIR %PACKAGES%

REM Custom Death Messages App
SET APPNAME=Distance Custom Death Messages - Editor
SET TARGET=%PACKAGES%\%APPNAME%

CALL :MAKEDIR %PACKAGES%\%APPNAME%
CALL :COPYDIR "%CD%\App.CustomDeathMessages.Windows\publish\win-x86" "%TARGET%\win-x86"
CALL :COPYDIR "%CD%\App.CustomDeathMessages.Windows\publish\win-x64" "%TARGET%\win-x64"
CALL :COPYDIR "%CD%\App.CustomDeathMessages.Linux\publish\linux-x64" "%TARGET%\linux-x64"

POPD
GOTO :EOF

REM ===== BUILD SUBROUTINES  =====

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