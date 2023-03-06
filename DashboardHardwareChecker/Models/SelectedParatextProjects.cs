using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardHardwareChecker.Models
{
    public class SelectedParatextProjects : Screen
    {
        //public bool IsSelected { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value; 
                NotifyOfPropertyChange(() => IsSelected);
            }
        }


        public string Name { get; set; } = "";
        public string LongName { get; set; } = "";
    }
}
