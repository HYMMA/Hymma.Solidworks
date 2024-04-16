@echo off

pushd %~dp0

echo Initializing dev environment

for /f "usebackq delims=" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -version [17.0^,18.0^) -property installationPath`) do (
  call "%%i\Common7\Tools\vsdevcmd.bat" -no_logo
)

echo Build

nuget restore QrifyInstaller.wixproj

msbuild -nologo -m QrifyInstaller.wixproj -p:BuildVersion=0.54 -p:AddinGuid={2EB85AF6-DB51-46FB-B955-D4A7708DA315} -p:Platform=X64 -p:Configuration=Release -p:CreateNugetPackage=false

pause
