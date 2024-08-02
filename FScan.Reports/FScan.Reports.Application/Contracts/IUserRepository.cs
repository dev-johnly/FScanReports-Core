using FScan.Reports.Application.Models.DTOs;
using FScan.Reports.Application.Models.Requests;
using FScan.Reports.Application.Models.Responses;
using FScan.Reports.Application.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Application.Contracts
{
    public interface IUserRepository
    {
        Task<Response> ChangePasswordAsync(ChangePasswordVM vm);
        Task<ResetPasswordDetailsDTO> ResetPasswordDetailsAsync(string ID);
        Task<Response> ResetPasswordAsync(PasswordResetRequest request);
        Task<Response> FChangePasswordAsync(ChangePasswordVM vm);
        Task<Response> EmailRegistrationAsync(EmailRegistrationVM vm);
        Task<Response> ForgotPasswordRequestAsync(ForgotPasswordVM vm);
    }
}
