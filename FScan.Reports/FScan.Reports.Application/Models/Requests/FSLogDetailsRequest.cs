using FScan.Reports.Application.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.Requests;

public class FSLogDetailsRequest
{
    public string ID { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    //public FSLogsDetailsDTO Sheet { get; set; }
    public bool Set { get; set; }
}
