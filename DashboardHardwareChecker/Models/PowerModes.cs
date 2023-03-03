using Caliburn.Micro;
using System;

namespace DashboardHardwareChecker.Models
{
    public class PowerModes : PropertyChangedBase
    {
        public Guid PowerModeGuid { get; set; }
        public string Name { get; set; } = string.Empty;

        public bool IsStandard { get; set; }


        private bool _isPresent;
        public bool IsPresent
        {
            get => _isPresent;
            set
            {
                _isPresent = value;
                NotifyOfPropertyChange(() => IsPresent);
            }
        }


        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                NotifyOfPropertyChange(() => IsActive);
            }
        }


        private string _runTime = "";
        public string RunTime
        {
            get => _runTime;
            set
            {
                _runTime = value;
                NotifyOfPropertyChange(() => RunTime);
            }
        }



    }
}
