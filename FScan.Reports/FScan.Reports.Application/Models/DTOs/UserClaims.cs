using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.DTOs
{
    public class UserClaims
    {
        public int IndexKey { get; set; }
        public string? ID { get; set; } = string.Empty;

        public string? Name { get; set; } =  string.Empty;

        public string? AccessType { get; set; } = string.Empty;

        public string? Email { get; set; }

    }
}
