using FScan.Reports.Application.Models.DTOs;
using FScan.Reports.Application.Models.Requests;
using FScan.Reports.Application.Models.Responses;
using FScan.Reports.Application.Models.ViewModels;
using FScanReports.Client.Contracts;
using System.Net.Http;
using System.Net.Http.Json;

namespace FScanReports.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response> ChangePasswordAsync(ChangePasswordVM vm)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/ChangePassword", vm);
            var result = await response.Content.ReadFromJsonAsync<Response>();
            return result!;
        }

        public async Task<ResetPasswordDetailsDTO> ResetPasswordDetailsAsync(string ID)
        {
            var response = await _httpClient.GetAsync($"api/User/ResetPasswordDetails/{ID}");
            var result = await response.Content.ReadFromJsonAsync<ResetPasswordDetailsDTO>();
            return result!;
        }

        public async Task<Response> ResetPasswordAsync(PasswordResetRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/ResetPassword/",request);
            var result = await response.Content.ReadFromJsonAsync<Response>();
            return result!;
        }

        public async Task<Response> FChangePasswordAsync(FChangePasswordVM vm)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/FChangePassword/", vm);
            var result = await response.Content.ReadFromJsonAsync<Response>();
            return result!;
        }

        public async Task<Response> EmailRegistrationAsync(EmailRegistrationVM vm)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/EmailRegistration/", vm);
            var result = await response.Content.ReadFromJsonAsync<Response>();
            return result!;
        }

        public async Task<Response> ForgotPasswordRequestAsync(ForgotPasswordVM vm)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/ForgotPasswordRequest/", vm);
            var result = await response.Content.ReadFromJsonAsync<Response>();
            return result!;
        }
    }
}
