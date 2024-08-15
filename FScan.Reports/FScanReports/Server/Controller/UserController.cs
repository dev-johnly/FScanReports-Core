﻿using FScan.Reports.Application.Contracts;
using FScan.Reports.Application.Models.Requests;
using FScan.Reports.Application.Models.ViewModels;
using FScanReports.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace FScanReports.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _user;

        public UserController(IUserRepository user)
        {
            _user = user;

        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordVM vm)
        {
            var result = await _user.ChangePasswordAsync(vm);
            return Ok(result);

        }

        [Authorize]
        [HttpGet("ResetPasswordDetails/{ID}")]
        public async Task<IActionResult> ResetPasswordDetailsAsync(string ID)
        {
            var result = await _user.ResetPasswordDetailsAsync(ID);
            return Ok(result);

        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] PasswordResetRequest request)
        {
            var result = await _user.ResetPasswordAsync(request);
            return Ok(result);

        }


        [HttpPost("FChangePassword")]
        public async Task<IActionResult> FChangePasswordAsync([FromBody] FChangePasswordVM vm)
        {
            var result = await _user.FChangePasswordAsync(vm);
            return Ok(result);

        }


        [HttpPost("EmailRegistration")]
        public async Task<IActionResult> EmailRegistrationAsync([FromBody] EmailRegistrationVM vm)
        {
            var result = await _user.EmailRegistrationAsync(vm);
            return Ok(result);

        }

        [HttpPost("ForgotPasswordRequest")]
        public async Task<IActionResult> ForgotPasswordRequestAsync([FromBody] ForgotPasswordVM vm)
        {
            var result = await _user.ForgotPasswordRequestAsync(vm);
            return Ok(result);

        }
    }
}
