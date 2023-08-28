@ECHO OFF

set "driveInno=C"
set /p driveInno=Drive for Inno?: 
if /i "%driveInno%" == "" goto :eof

set CURRENTPATH=%cd%
set /p PASSWORD=<"..\code_signing_key\password.txt"
rem get the absolute path to the relative key file
CALL :NORMALIZEPATH "..\code_signing_key\ClearBible.pfx"

echo ========== BUILD 64-Bit Version of Plugin ==============
cd ..\DashboardHardwareChecker
dotnet clean --configuration Release
rem dotnet build  --configuration Release
dotnet publish  --configuration Release

echo code sign the WPF exe	
 ..\code_signing_key\signing_tool\signtool.exe ^
 	sign /v /f %RETVAL% ^
 	/p "%PASSWORD%" ^
 	/t http://timestamp.comodoca.com/authenticode ^
"%CURRENTPATH%\..\DashboardHardwareChecker\bin\Release\net7.0-windows\DashboardHardwareChecker.exe"


cd ..\installer

pause

::===================INNO Dashboard=====================
echo run the Inno Setup Compliler on Dashboard
"%driveInno%:\Program Files (x86)\Inno Setup 6\ISCC.exe" "%CURRENTPATH%\DashboardHardwareInstaller.iss"

echo code sign the Dashboard installer
..\code_signing_key\signing_tool\signtool.exe ^
	sign /v /f %RETVAL% ^
	/p "%PASSWORD%" ^
	/t http://timestamp.comodoca.com/authenticode ^
	"%CURRENTPATH%\Output\DashboardHardwareChecker_Setup.exe"

pause


:: ========== FUNCTIONS ==========
EXIT /B

:NORMALIZEPATH
  SET RETVAL=%~f1
  EXIT /B

:GET_THIS_DIR
pushd %~dp0
set THIS_DIR=%CD%
popd
goto :EOF