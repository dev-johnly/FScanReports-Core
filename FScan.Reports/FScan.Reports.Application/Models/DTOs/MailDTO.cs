using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.DTOs
{
    public class MailDTO
    {
        public string MailTo { get; set; }
        public string MailFrom { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
