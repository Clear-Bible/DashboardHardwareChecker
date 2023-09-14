using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardHardwareChecker.Models
{
    public partial class User 
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [NotMapped]
        public string? FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public string? ParatextUserName { get; set; }

        [NotMapped]
        public bool? IsInternal { get; set; } = false;

        [NotMapped]
        public int? LicenseVersion { get; set; } = 0;

        public int? LastAlignmentLevelId { get; set; }

        public Guid? DefaultLabelGroupId { get; set; }

    }
}
