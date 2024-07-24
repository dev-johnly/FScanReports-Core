using FScan.Reports.Application.Models.Identity;
using FScan.Reports.Application.Models.Responses;
using FScan.Reports.Application.Models.ViewModels;

namespace FScanReports.Client.Contracts
{
    public interface IAuthenticationService
    {
        Task<AuthResponse> LoginAsync(LoginVM vm);

    }
}
