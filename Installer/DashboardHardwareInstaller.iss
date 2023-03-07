; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "ClearDashboardHardwareChecker"
#define MyAppVersion "1.0.0.0"
#define MyAppPublisher "Clear Bible, Inc."
#define MyAppURL "https://www.clear.bible/"
#define MyAppExeName "DashboardHardwareChecker.exe"
#define MyAppAssocName MyAppName + " File"
#define MyAppAssocExt ".myp"
#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{A5CFC325-53E9-4D7C-A3DC-B177C5F9D358}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={commonpf64}\{#MyAppName}
ChangesAssociations=yes
DisableProgramGroupPage=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputBaseFilename=DashboardHardwareChecker_Setup
SetupIconFile=installer.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64
UninstallDisplayIcon={uninstallexe}
Uninstallable=yes
;UninstallIconFile=..\DashboardHardwareChecker\icon.ico
DisableDirPage=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";

[Files]
Source: "windowsdesktop-runtime-7.0.3-win-x64.exe"; Flags: dontcopy noencryption
Source: "..\DashboardHardwareChecker\bin\Release\net7.0-windows\publish\win-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

; NOTE: Don't use "Flags: ignoreversion" on any shared system files


[Registry]

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
function InitializeSetup: Boolean;
var
    ResultCode: Integer;
    Value: string;
begin
  if IsWin64 then begin
   // install the .NET Runtime
     if not DirExists('C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App\7.0.3\') then begin
      if not FileExists(ExpandConstant('{tmp}{\}') + 'windowsdesktop-runtime-7.0.3-win-x64.exe') then begin          
        ExtractTemporaryFile('windowsdesktop-runtime-7.0.3-win-x64.exe');
      end;
     Result := ShellExec('', ExpandConstant('{tmp}{\}') + 'windowsdesktop-runtime-7.0.3-win-x64.exe', '/passive', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) and (ResultCode = 0);
    end;
    Result := true;
  end;

end;

