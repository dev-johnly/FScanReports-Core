using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.Identity
{
    public class AuthRequest
    {
        public string? Usercode { get; set; } = string.Empty;
        public string? Password { get; set; }
    }
}
