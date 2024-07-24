using FScan.Reports.Application.Contracts;
using FScan.Reports.Application.Models.Identity;
using FScan.Reports.Application.Models.Responses;
using FScan.Reports.Application.Models.ViewModels;
using FScan.Reports.Domain.Entities;
using FScan.Reports.Infrastructure.Data;
using FScan.Reports.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Infrastructure.Repositories;

public class AuthenticationRepository : GenericRepository<NGAC_USERINFO>, IAuthenticationRepository
{
    private readonly IConfiguration _configuration;

    public AuthenticationRepository(FScanContext context,IConfiguration configuration) : base(context)
    { 
     _configuration = configuration;

    }

    public async Task<AuthResponse> LoginAsync(LoginVM vm)
    {
        AuthResponse response = new();
        try
        {
            var user = await _context.NGAC_USERINFO
                                    .Where(s => s.ID.Trim() == vm.Usercode.Trim())
                                    .FirstOrDefaultAsync();

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found, sorry";
                return response;
            }

            string decryptedPassword = Encryptor.Decrypt(user.FSPassword);

            if (vm.Password == "Defaul32#$%!" && decryptedPassword == "Defaul32#$%!")
            {
                response.FirstLogin = true;
                response.UserCode = vm.Usercode;
                return response;
                //redirect to changepassword page here
            }

            if (user.AccessType == null)
            {
                user.AccessType = "User";
                await _context.SaveChangesAsync();
            }

            if (decryptedPassword.Equals(vm.Password))
            {
                response.FirstLogin = false;
                response.Success = true;
                response.Message = "Login Successfully";
                response.Token = GenerateJWTToken(user);
                //response.Principal = AuthenticateUser(user); // Add ClaimsPrincipal to response
            }
            else
            {
                response.FirstLogin = false;
                response.Success = false;
                response.Message = $"Credentials for '{vm.Usercode}' aren't valid.";
            }

            return response;
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = "An error occurred during login.";
            return response;
        }
    }

    private string GenerateJWTToken(NGAC_USERINFO user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.ID),
        new Claim(ClaimTypes.Name, user.Name!),
        new Claim(ClaimTypes.Role, user.AccessType!),
        new Claim(ClaimTypes.Email, user.BankcomEmail!)
    };
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddHours(int.Parse(_configuration["Jwt:TokenExpiryHours"])),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    //private ClaimsPrincipal AuthenticateUser(NGAC_USERINFO user)
    //{
    //    var claims = new List<Claim>
    //{
    //    new Claim(ClaimTypes.NameIdentifier, user.ID),
    //    new Claim(ClaimTypes.Name, user.Name!),
    //    new Claim(ClaimTypes.Role, user.AccessType!)
    //};

    //    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    //    return new ClaimsPrincipal(identity);
    //}
}
