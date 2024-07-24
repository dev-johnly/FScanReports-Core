using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Models.Responses
{
    public class AuthResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool FirstLogin { get; set; }
        public string? Message { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string UserCode { get; set; }

    }
}
