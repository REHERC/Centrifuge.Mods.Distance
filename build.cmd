@ECHO OFF
REM https://psycodedeveloper.wordpress.com/2019/07/02/locating-and-running-msbuild-from-the-command-line-in-visual-studio-2019/

PUSHD %~dp0

build_dotnet.cmd

SET SOLUTION=%CD%\Centrifuge.Mods.Distance.sln

SET VS_DIR=

IF exist "%programfiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" (
  FOR /F "tokens=* USEBACKQ" %%F in (`"%programfiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -property installationPath`) do set VS_DIR=%%F
)

REM ECHO %VS_DIR%
REM EXIT /B 0

PUSHD "%VS_DIR%\Common7\Tools"

CALL VsDevCmd.bat
msbuild "%SOLUTION%" /t:Rebuild /m /v:m /p:Configuration=Release -maxcpucount:4

POPD
POPD