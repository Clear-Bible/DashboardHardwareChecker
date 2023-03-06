using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardHardwareChecker.Models
{
    public class ParatextBook
    {
        public string? BookId { get; set; }
        public string? BookNameShort { get; set; }
        public bool Available { get; set; }
        public string? FilePath { get; set; }
        public int USFM_Num { get; set; }

    }
}
