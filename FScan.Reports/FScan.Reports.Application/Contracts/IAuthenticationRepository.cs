using FScan.Reports.Application.Models.Identity;
using FScan.Reports.Application.Models.Responses;
using FScan.Reports.Application.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Contracts
{
    public interface IAuthenticationRepository
    {
        Task<AuthResponse> LoginAsync(LoginVM vm);
    }
}
