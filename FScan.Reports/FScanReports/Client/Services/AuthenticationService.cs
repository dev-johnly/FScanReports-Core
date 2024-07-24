using FScan.Reports.Application.Models.Identity;
using FScan.Reports.Application.Models.Responses;
using FScan.Reports.Application.Models.ViewModels;
using FScanReports.Client.Contracts;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using System.Net.Http.Json;


namespace FScanReports.Client.Services
{
    public class AuthenticationService : Contracts.IAuthenticationService
    {
        private readonly HttpClient _httpClient;

        public AuthenticationService(HttpClient httpClient) {
        
            _httpClient = httpClient;
        }
        public async Task<AuthResponse> LoginAsync(LoginVM vm)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Authentication/Login", vm);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return result!;
        }
    }
}
