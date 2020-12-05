@ECHO OFF
PUSHD %~dp0

REM CustomDeathMessages App

PUSHD App.CustomDeathMessages.Windows
dotnet publish -p:PublishProfile=Properties\PublishProfiles\win-x86.pubxml
dotnet publish -p:PublishProfile=Properties\PublishProfiles\win-x64.pubxml
POPD

PUSHD App.CustomDeathMessages.Linux
dotnet publish -p:PublishProfile=Properties\PublishProfiles\linux-x64.pubxml
POPD

POPD