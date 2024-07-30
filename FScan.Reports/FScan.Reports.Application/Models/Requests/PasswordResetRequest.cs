using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.Requests;

public class PasswordResetRequest
{
    public string ID { get; set; }
    public string BankcomEmail { get; set; }
}
