using Caliburn.Micro;
using DashboardHardwareChecker.Helpers;
using DashboardHardwareChecker.Models;
using MahApps.Metro.Converters;
using PowerManagerAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DashboardHardwareChecker.ViewModels
{
    public class ShellViewModel : Screen
    {
        #region Member Variables   

        public enum Chassis
        {
            Desktop,
            Laptop
        }

        private enum OsType
        {
            Win10,
            Win10Pro,
            Win11,
            Win11Pro,
            Win11ProWorkstations,
        }

        public enum PluggedInStatus
        {
            PowerPlug,
            PowerPlugOffOutline
        }

        public enum BatteryCharge
        {
            BatteryChargingOutline,
            BatteryCharging10,
            BatteryCharging20,
            BatteryCharging30,
            BatteryCharging40,
            BatteryCharging50,
            BatteryCharging60,
            BatteryCharging70,
            BatteryCharging80,
            BatteryCharging90,
            BatteryCharging100,
            Battery10,
            Battery20,
            Battery30,
            Battery40,
            Battery50,
            Battery60,
            Battery70,
            Battery80,
            Battery90,
            Battery100,
            BatteryOffOutline,
        }

        private OsType _osType = OsType.Win10;

        private DispatcherTimer _timer = new();

        private string _zipPathAttachment { get; set; } = string.Empty;

        #endregion //Member Variables



        #region Public Properties

        public string ParatextUser { get; set; } = string.Empty;
        public List<string> Files { get; set; } = new();

        #endregion //Public Properties


        #region Observable Properties

        private string? _version;
        public string? Version
        {
            get => _version;
            set => Set(ref _version, value);
        }


        private bool _isSendMessageGroupBoxEnabled;
        public bool IsSendMessageGroupBoxEnabled
        {
            get => _isSendMessageGroupBoxEnabled;
            set
            {
                _isSendMessageGroupBoxEnabled = value;
                NotifyOfPropertyChange(() => IsSendMessageGroupBoxEnabled);
            }
        }



        private Visibility _progressBarVisibility = Visibility.Collapsed;
        public Visibility ProgressBarVisibility
        {
            get => _progressBarVisibility;
            set
            {
                _progressBarVisibility = value;
                NotifyOfPropertyChange(() => ProgressBarVisibility);
            }
        }



        private Cursor _cursor;
        public Cursor Cursor
        {
            get => _cursor;
            set
            {
                _cursor = value;
                NotifyOfPropertyChange(() => Cursor);
            }
        }


        private long _primeNumber = 5555555555;
        public long PrimeNumber
        {
            get => _primeNumber;
            set
            {
                _primeNumber = value;
                NotifyOfPropertyChange(() => PrimeNumber);
            }
        }

        private string _result = "";
        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                NotifyOfPropertyChange(() => Result);
            }
        }




        private PluggedInStatus _powerStatus;
        public PluggedInStatus PowerStatus
        {
            get => _powerStatus;
            set
            {
                _powerStatus = value;
                NotifyOfPropertyChange(() => PowerStatus);
            }
        }



        private string _machineName = Environment.MachineName;
        public string MachineName
        {
            get => _machineName;
            set
            {
                _machineName = value;
                NotifyOfPropertyChange(() => MachineName);
            }
        }



        private string _batteryPercent;
        public string BatteryPercent
        {
            get => _batteryPercent;
            set
            {
                _batteryPercent = value;
                NotifyOfPropertyChange(() => BatteryPercent);
            }
        }


        private Chassis _chassisType = Chassis.Desktop;
        public Chassis ChassisType
        {
            get => _chassisType;
            set
            {
                _chassisType = value;
                NotifyOfPropertyChange(() => ChassisType);
            }
        }



        private ObservableCollection<PowerModes> _powerModes = new();
        public ObservableCollection<PowerModes> PowerModes
        {
            get => _powerModes;
            set
            {
                _powerModes = value;
                NotifyOfPropertyChange(() => PowerModes);
            }
        }


        private string _windowsVersion;
        public string WindowsVersion
        {
            get => _windowsVersion;
            set
            {
                _windowsVersion = value;
                NotifyOfPropertyChange(() => WindowsVersion);
            }
        }

        private BatteryCharge _batteryChargeStatus = BatteryCharge.BatteryOffOutline;
        public BatteryCharge BatteryChargeStatus
        {
            get => _batteryChargeStatus;
            set
            {
                _batteryChargeStatus = value;
                NotifyOfPropertyChange(() => BatteryChargeStatus);
            }
        }

        private bool _isNotRunning = true;
        public bool IsNotRunning
        {
            get => _isNotRunning;
            set
            {
                _isNotRunning = value;
                NotifyOfPropertyChange(() => IsNotRunning);
            }
        }


        private string _userName = "";
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                ValidateOkButton();
            }
        }

        private string _userEmail = "";
        public string UserEmail
        {
            get => _userEmail;
            set
            {
                _userEmail = value;
                NotifyOfPropertyChange(() => UserEmail);
                ValidateOkButton();
            }
        }

        private string _organization = "";
        public string Organization
        {
            get => _organization;
            set
            {
                _organization = value;
                NotifyOfPropertyChange(() => Organization);
                ValidateOkButton();
            }
        }


        private Visibility _noInternetVisibility = Visibility.Collapsed;
        public Visibility NoInternetVisibility
        {
            get => _noInternetVisibility;
            set
            {
                _noInternetVisibility = value;
                NotifyOfPropertyChange(() => NoInternetVisibility);
            }
        }

        private Visibility _sendErrorVisibility = Visibility.Collapsed;
        public Visibility SendErrorVisibility
        {
            get => _sendErrorVisibility;
            set
            {
                _sendErrorVisibility = value;
                NotifyOfPropertyChange(() => SendErrorVisibility);
            }
        }

        private Visibility _sendSuccessfulVisibility = Visibility.Collapsed;
        public Visibility SendSuccessfulVisibility
        {
            get => _sendSuccessfulVisibility;
            set
            {
                _sendSuccessfulVisibility = value;
                NotifyOfPropertyChange(() => SendSuccessfulVisibility);
            }
        }


        private bool _isTestComplete;
        public bool IsTestComplete
        {
            get => _isTestComplete;
            set
            {
                _isTestComplete = value;
                NotifyOfPropertyChange(() => IsTestComplete);
                ValidateOkButton();
            }
        }


        private bool _okButtonIsEnabled;
        public bool OkButtonIsEnabled
        {
            get => _okButtonIsEnabled;
            set
            {
                _okButtonIsEnabled = value;
                NotifyOfPropertyChange(() => OkButtonIsEnabled);
            }
        }


        private string _computerLocation;
        public string ComputerLocation
        {
            get => _computerLocation;
            set
            {
                _computerLocation = value; 
                NotifyOfPropertyChange(() => ComputerLocation);
            }
        }

        private ObservableCollection<SelectedParatextProjects> _selectedProjects = new();
        public ObservableCollection<SelectedParatextProjects> SelectedProjects
        {
            get => _selectedProjects;
            set
            {
                _selectedProjects = value;
                NotifyOfPropertyChange(() => SelectedProjects);
            }
        }


        private int _projectCount = 0;
        public int ProjectCount
        {
            get => _projectCount;
            set
            {
                _projectCount = value;
                NotifyOfPropertyChange(() => ProjectCount);
            }
        }



        private string _paratextVersion = "";
        public string ParatextVersion
        {
            get => _paratextVersion;
            set
            {
                _paratextVersion = value;
                NotifyOfPropertyChange(() => ParatextVersion);
            }
        }



        #endregion //Observable Properties


        #region Constructor

        public ShellViewModel()
        {
            // setup timer
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        
        protected override async void OnViewLoaded(object view)
        {
            //get the assembly version
            var thisVersion = Assembly.GetEntryAssembly().GetName().Version;
            Version = $"Clear Dashboard System Checker  -  Version: {thisVersion.Major}.{thisVersion.Minor}.{thisVersion.Build}.{thisVersion.Revision}";


            // figure out the Paratext version
            ParatextProxy paratextProxy = new ParatextProxy();
            if (paratextProxy.IsParatextInstalled())
            {
                var path = Path.Combine(paratextProxy.ParatextInstallPath, "paratext.exe");
                if (File.Exists(path))
                {
                    var fileInfo = FileVersionInfo.GetVersionInfo(path);
                    ParatextVersion = fileInfo.ProductVersion;
                }
                else
                {
                    ParatextVersion = "Not Installed";
                }

            }


            // Get the OS Version
            using (var objOS = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
            {
                foreach (ManagementObject objMgmt in objOS.Get())
                {
                    foreach (var x in objMgmt.Properties)
                    {
                        Debug.WriteLine(x.Name + " ::: " + x.Value);
                    }

                    WindowsVersion = String.Format("{0}", objMgmt.Properties["Caption"].Value);

                    switch (objMgmt.Properties["Caption"].Value.ToString().Trim())
                    {
                        case "Microsoft Windows 10":
                            _osType = OsType.Win10;
                            break;
                        case "Microsoft Windows 10 Pro":
                            _osType = OsType.Win10Pro;
                            break;
                        case "Microsoft Windows 11":
                            _osType = OsType.Win11;
                            break;
                        case "Microsoft Windows 11 Pro":
                            _osType = OsType.Win11Pro;
                            break;
                        case "Microsoft Windows 11 Pro for Workstations":
                            _osType = OsType.Win11ProWorkstations;
                            break;
                    }
                }
            }

            InitializeDefaultPowerModes();
            GetBattery();

            var bRet = await NetworkHelper.IsConnectedToInternet();
            if (bRet == false)
            {
                NoInternetVisibility = Visibility.Visible;
            }
            else
            {
                NoInternetVisibility = Visibility.Collapsed;

                await GetComputerGeolocation();
            }


            // get the list of paratext projects that the user has
            paratextProxy.GetParatextProjectsPath();
            if (paratextProxy.ParatextProjectPath != "")
            {
                var projects = await paratextProxy.GetParatextProjectsOrResources();
                if (projects is not null)
                {
                    foreach (var project in projects)
                    {
                        SelectedProjects.Add(new SelectedParatextProjects
                        {
                            IsSelected = false,
                            Name = project.Name,
                            LongName = project.LongName,
                        });
                    }
                }
            }

            base.OnViewLoaded(view);
        }

        #endregion //Constructor



        #region Methods

        public void ValidateOkButton()
        {
            if (UserName == "" || UserEmail == "" || Organization == "" || IsTestComplete == false)
            {
                return;
            }
            OkButtonIsEnabled = true;
        }

        public void CalculateTotals()
        {
            ProjectCount = _selectedProjects.Where(x => x.IsSelected).ToList().Count;
        }


        private void InitializeDefaultPowerModes()
        {
            _powerModes.Clear();

            // initialize the default power modes
            _powerModes.Add(new PowerModes
            {
                PowerModeGuid = Guid.Parse("a1841308-3541-4fab-bc81-f71556f20b4a"),
                IsPresent = false,
                Name = "Power Saver",
                IsStandard = true
            });
            _powerModes.Add(new PowerModes
            {
                PowerModeGuid = Guid.Parse("381b4222-f694-41f0-9685-ff5bb260df2e"),
                IsPresent = false,
                Name = "Balanced",
                IsStandard = true
            });
            _powerModes.Add(new PowerModes
            {
                PowerModeGuid = Guid.Parse("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"),
                IsPresent = false,
                Name = "High Performance",
                IsStandard = true
            });

            // Ultimate is only available in WorkStation SKUs
            if (_osType == OsType.Win11ProWorkstations)
            {
                _powerModes.Add(new PowerModes
                {
                    PowerModeGuid = Guid.Parse("e9a42b02-d5df-448d-aa00-03f14749eb61"),
                    IsPresent = false,
                    Name = "Ultimate Performance",
                    IsStandard = true
                });
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            GetBatteryPercent();
        }

        private void GetBatteryPercent()
        {
            // are we plugged in?
            PowerState state = PowerState.GetPowerState();

            if (state.ACLineStatus == ACLineStatus.Offline)
            {
                PowerStatus = PluggedInStatus.PowerPlugOffOutline;
            }
            else
            {
                PowerStatus = PluggedInStatus.PowerPlug;
            }

            if (state.BatteryFlag == BatteryFlag.NoSystemBattery)
            {
                BatteryPercent = "None Present";
                BatteryChargeStatus = BatteryCharge.BatteryOffOutline;
                return;
            }


            // get the batteries
            ManagementClass wmi = new ManagementClass("Win32_Battery");
            var allBatteries = wmi.GetInstances();

            foreach (var battery in allBatteries)
            {
                int estimatedChargeRemaining = Convert.ToInt32(battery["EstimatedChargeRemaining"]);
                BatteryPercent = estimatedChargeRemaining.ToString() + " %";
                if (state.ACLineStatus == ACLineStatus.Online)
                {
                    // we are charging
                    BatteryChargeStatus = estimatedChargeRemaining switch
                    {
                        var i when i < 10 => BatteryCharge.BatteryChargingOutline,
                        var i when i >= 10 && i < 20 => BatteryCharge.BatteryCharging10,
                        var i when i >= 20 && i < 30 => BatteryCharge.BatteryCharging20,
                        var i when i >= 30 && i < 40 => BatteryCharge.BatteryCharging30,
                        var i when i >= 40 && i < 50 => BatteryCharge.BatteryCharging40,
                        var i when i >= 50 && i < 60 => BatteryCharge.BatteryCharging50,
                        var i when i >= 60 && i < 70 => BatteryCharge.BatteryCharging60,
                        var i when i >= 70 && i < 80 => BatteryCharge.BatteryCharging70,
                        var i when i >= 80 && i < 90 => BatteryCharge.BatteryCharging80,
                        var i when i >= 90 && i < 100 => BatteryCharge.BatteryCharging90,
                        var i when i == 100 => BatteryCharge.BatteryCharging100,
                    };

                }
                else
                {
                    BatteryChargeStatus = estimatedChargeRemaining switch
                    {
                        var i when i < 10 => BatteryCharge.BatteryChargingOutline,
                        var i when i >= 10 && i < 20 => BatteryCharge.Battery10,
                        var i when i >= 20 && i < 30 => BatteryCharge.Battery20,
                        var i when i >= 30 && i < 40 => BatteryCharge.Battery30,
                        var i when i >= 40 && i < 50 => BatteryCharge.Battery40,
                        var i when i >= 50 && i < 60 => BatteryCharge.Battery50,
                        var i when i >= 60 && i < 70 => BatteryCharge.Battery60,
                        var i when i >= 70 && i < 80 => BatteryCharge.Battery70,
                        var i when i >= 80 && i < 90 => BatteryCharge.Battery80,
                        var i when i >= 90 && i < 100 => BatteryCharge.Battery90,
                        var i when i == 100 => BatteryCharge.Battery100,
                    };

                }
            }
        }

        public void GetBattery()
        {
            var output = GetBatterySpecs();


            // get the active plan
            var activePlanGuid = PowerManager.GetActivePlan();

            // grab the list of what power plans are available
            var list = PowerManager.ListPlans();

            foreach (var plan in list)
            {
                var name = PowerManager.GetPlanName(plan);
                output += $"Power Management Plan: {name} : {plan}\n";

                var mode = _powerModes.FirstOrDefault(x => x.PowerModeGuid == plan);
                if (mode is null)
                {
                    PowerModes powerModes = new PowerModes
                    {
                        IsPresent = true,
                        Name = name,
                        PowerModeGuid = plan
                    };

                    if (plan == activePlanGuid)
                    {
                        powerModes.IsActive = true;
                    }

                    _powerModes.Add(powerModes);

                }
                else
                {
                    mode.IsPresent = true;

                    if (mode.PowerModeGuid == activePlanGuid)
                    {
                        mode.IsActive = true;
                    }

                }

            }


            Debug.WriteLine($"ACTIVE PLAN: {PowerManager.GetPlanName(activePlanGuid)}");
            output += $"\nActive Plan: {PowerManager.GetPlanName(activePlanGuid)} : {activePlanGuid}";
        }

        private string GetBatterySpecs()
        {
            var output = "";
            //Retrieves a collection of system management objects based on below query !
            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_Battery");
            foreach (ManagementObject mo in mos.Get())
            {
                // To print the Battery name
                output += $"Battery Name\t:{mo["Name"]}\n";
                // To print the Battery charging status
                output += $"Charge \t\t:{mo["EstimatedChargeRemaining"]}%\n\n";
            }


            PowerState state = PowerState.GetPowerState();
            output += $"AC Line: {state.ACLineStatus}\n";
            output += $"Battery: {state.BatteryFlag}\n";
            output += $"Battery life %: {state.BatteryLifePercent}\n\n";

            if (state.BatteryFlag == BatteryFlag.NoSystemBattery)
            {
                ChassisType = Chassis.Desktop;
            }
            else
            {
                ChassisType = Chassis.Laptop;
            }

            return output;
        }

        public async void RunPrimeNumbers()
        {
            await RunTest();
        }

        private async Task RunTest()
        {
            ProgressBarVisibility = Visibility.Visible;
            Result = "WORKING...";
            Cursor = Cursors.Wait;
            IsNotRunning = false;

            Stopwatch stopwatch = Stopwatch.StartNew();
            long a = 0;

            await Task.Run(() =>
            {
                // calculate prime numbers
                for (long i = 1; i <= PrimeNumber; i++)
                {
                    if (PrimeNumber % i == 0)
                    {
                        a++;
                    }
                }
            });

            stopwatch.Stop();
            var elapsed = stopwatch.ElapsedMilliseconds;

            if (a == 2)
            {
                Result = $"{PrimeNumber} is a Prime Number: {elapsed} ms";
            }
            else
            {
                Result = $"{PrimeNumber} is NOT a Prime Number: {elapsed} ms";
            }

            foreach (var powerMode in PowerModes)
            {
                if (powerMode.IsActive)
                {
                    powerMode.RunTime = $"{elapsed} ms";
                    NotifyOfPropertyChange(() => PowerModes);
                    break;
                }
            }

            Cursor = null;
            ProgressBarVisibility = Visibility.Collapsed;
            IsNotRunning = true;
        }

        public async void RunAll()
        {
            foreach (var powerMode in PowerModes)
            {
                if (powerMode.IsPresent)
                {
                    SetPowerMode(powerMode);
                    await RunTest();
                }
            }

            PlaySound.PlaySoundFromResource(SoundType.Success);

            IsSendMessageGroupBoxEnabled = true;




            // get computer info for a file
            ComputerInfo computerInfo = new();
            var destinationComputerInfoPath = Path.Combine(Path.GetTempPath(), "computerInfo.log");

            _ = await computerInfo.GetComputerInfo(destinationComputerInfoPath);
            Files.Add(destinationComputerInfoPath);

            // get power plan info
            StringBuilder sb = new StringBuilder();
            foreach (var powerMode in PowerModes)
            {
                sb.AppendLine($"Name: {powerMode.Name}");
                sb.AppendLine($"Is Present: {powerMode.IsPresent.ToString()}");
                sb.AppendLine($"Is Active: {powerMode.IsActive.ToString()}");
                sb.AppendLine($"Guid: {powerMode.PowerModeGuid.ToString()}");
                sb.AppendLine($"Is Standard: {powerMode.IsStandard.ToString()}");
                sb.AppendLine($"Time (ms): {powerMode.RunTime}");
                sb.AppendLine();
            }
            var destinationPowerPlanPath = Path.Combine(Path.GetTempPath(), "PowerPlan.log");
            File.WriteAllText(destinationPowerPlanPath, sb.ToString());
            Files.Add(destinationPowerPlanPath);


            // create the zip file
            var guid = Guid.NewGuid().ToString();
            _zipPathAttachment = Path.Combine(Path.GetTempPath(), $"{guid}.zip");
            // zip up everything
            if (Files.Count > 0)
            {
                if (File.Exists(_zipPathAttachment))
                {
                    File.Delete(_zipPathAttachment);
                }

                ZipFiles zipFiles = new(Files, _zipPathAttachment);
                var succcess = zipFiles.Zip();

                if (succcess == false)
                {
                    //_logger.LogError("Error zipping files");
                }
            }

            IsTestComplete = true;
        }


        public void SetPowerMode(PowerModes powerMode)
        {
            if (powerMode.IsPresent == false && powerMode.IsStandard)
            {
                // restore missing default
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "powercfg",
                    Arguments = $"-duplicatescheme {powerMode.PowerModeGuid.ToString()}",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };
                process.Start();
                // Now read the value, parse to int and add 1 (from the original script)
                int online = int.Parse(process.StandardOutput.ReadToEnd()) + 1;
                process.WaitForExit();


            }

            PowerManager.SetActivePlan(powerMode.PowerModeGuid);

            // get the active plan
            var activePlanGuid = PowerManager.GetActivePlan();

            foreach (var mode in PowerModes)
            {
                if (mode.PowerModeGuid == activePlanGuid)
                {
                    mode.IsActive = true;
                    mode.IsPresent = true;
                }
                else
                {
                    mode.IsActive = false;
                }
            }
        }

        public async void Close()
        {
            await this.TryCloseAsync();
        }


        public async Task SendMessage()
        {
            var bRet = await NetworkHelper.IsConnectedToInternet();
            // check internet connection
            if (bRet == false)
            {
                //_logger.LogWarning("No internet connection available");
                return;
            }

            var thisVersion = Assembly.GetEntryAssembly().GetName().Version;
            var versionNumber = $"{thisVersion.Major}.{thisVersion.Minor}.{thisVersion.Build}.{thisVersion.Revision}";

            string msg = $"*Dashboard HardWareChecker Version*: {versionNumber} \n\n*Name:* {UserName} \n*Email:* {UserEmail} \n*Organization:* {Organization}";
            msg += $"\n*Paratext Version:* {ParatextVersion} \n\n*Location*:\n{ComputerLocation}\n";
            msg += $"\n---------------------------------------------";
            foreach (var projects in SelectedProjects)
            {
                if (projects.IsSelected)
                {
                    msg += $"\n*Paratext Project:* {projects.Name}  {projects.LongName}";
                }
            }

            SlackMessage slackMessage = new SlackMessage(msg, this._zipPathAttachment);
            var bSuccess = await slackMessage.SendFileToSlackAsync();

            if (bSuccess == true)
            {
                SendSuccessfulVisibility = Visibility.Visible;
                SendErrorVisibility = Visibility.Collapsed;
            }
            else
            {
                SendSuccessfulVisibility = Visibility.Collapsed;
                SendErrorVisibility = Visibility.Visible;
            }
        }

        private void GetParatextVersion()
        {

        }

        private async Task GetComputerGeolocation()
        {
            await Task.Run(() =>
            {
                var ip = GeoLocation.GetExternalIPAddress();
                if (ip == "")
                {
                    ComputerLocation = "";
                    return Task.CompletedTask;
                }

                ComputerLocation = GeoLocation.GetUserCountryByIp(ip);
                return Task.CompletedTask;
            });


            //var timeout = 1000;

            ////DONE: don't forget to dispose CancellationTokenSource instance 
            //using (var tokenSource = new CancellationTokenSource(timeout))
            //{
            //    try
            //    {
            //        var token = tokenSource.Token;

            //        //TODO: May be you'll want to add .ConfigureAwait(false);
            //        Task task = Task.Run(() =>
            //        {
            //            var ip = GeoLocation.GetExternalIPAddress();
            //        });
            //        Task task = Task.Run(() => DoSomething(token), token);

            //        await task;

            //        // Completed
            //    }
            //    catch (TaskCanceledException)
            //    {
            //        // Cancelled due to timeout

            //    }
            //    catch (Exception e)
            //    {
            //        // Failed to complete due to e exception

            //        Console.WriteLine("--StartThread ...there is an exception----");

            //        //DONE: let's be nice and don't swallow the exception
            //        throw;
            //    }
            //}







        }

        #endregion // Methods


    }
}
