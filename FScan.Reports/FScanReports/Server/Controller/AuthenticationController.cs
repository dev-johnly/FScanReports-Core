using FScan.Reports.Application.Contracts;
using FScan.Reports.Application.Models.Identity;
using FScan.Reports.Application.Models.ViewModels;
using FScan.Reports.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FScanReports.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _authentication;

        public AuthenticationController(IAuthenticationRepository authentication)
        { 
            _authentication = authentication;

        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginVM vm)
        {
            var result = await _authentication.LoginAsync(vm);
            return Ok(result);

        }

    }
}
