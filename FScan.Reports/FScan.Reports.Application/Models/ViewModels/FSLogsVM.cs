using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.ViewModels
{
    public class FSLogsVM
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string GroupName { get; set; }
        public string TerminalName { get; set; }
        public DateTime? Date { get; set; }
        public int? FunctionKey { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
