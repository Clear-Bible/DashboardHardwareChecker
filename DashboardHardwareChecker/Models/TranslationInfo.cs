using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardHardwareChecker.Models
{
    public class TranslationInfo
    {
        public CorpusType CorpusType { get; set; } = CorpusType.Unknown;
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectGuid { get; set; } = string.Empty;
    }
}
