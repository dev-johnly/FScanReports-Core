using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.Requests;

public class PdfRequest
{
    public DateTime? DateTo {  get; set; }
    public DateTime? DateFrom { get; set; } 
}
