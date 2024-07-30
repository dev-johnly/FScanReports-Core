using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.DTOs;


public class ResetPasswordDetailsDTO
{

    public long IndexKey { get; set; }
    public string ID { get; set; }
    public string Name { get; set; }
    public string GroupName { get; set; }
    public string AccessType { get; set; }
    public string BankcomEmail { get; set; }
}