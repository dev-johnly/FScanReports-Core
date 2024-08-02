using FScan.Reports.Application.Contracts;
using FScan.Reports.Application.Models.DTOs;
using FScan.Reports.Application.Models.Identity;
using FScan.Reports.Application.Models.Responses;
using FScan.Reports.Application.Models.ViewModels;
using FScan.Reports.Domain.Entities;
using FScan.Reports.Infrastructure.Data;
using FScan.Reports.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Infrastructure.Repositories;

public class AuthenticationRepository : GenericRepository<NGAC_USERINFO>, IAuthenticationRepository
{
    private readonly IConfiguration _configuration;
    private readonly UserClaimsGetter _userClaims;
    private readonly string? _ID;

    public AuthenticationRepository(FScanContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(context)
    {
        _configuration = configuration;
        _userClaims = new UserClaimsGetter(httpContextAccessor);
        _ID = _userClaims.GetClaims().ID;
    }

    public async Task<AuthResponse> LoginAsync(LoginVM vm)
    {
        AuthResponse response = new();
        try
        {
            var user = await _context.NGAC_USERINFO
                                     .FirstOrDefaultAsync(s => s.ID.Trim() == vm.Usercode.Trim());

            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid Username or password"
                };
            }

            if (user.AccessType == null)
            {
                user.AccessType = "User";
                await _context.SaveChangesAsync();
            }

            string decryptedPassword = Encryptor.Decrypt(user.FSPassword);
            bool isFirstLogin = vm.Password == "Defaul32#$%!" && decryptedPassword == "Defaul32#$%!";
            bool isAuthenticated = decryptedPassword.Equals(vm.Password);
          
            if (string.IsNullOrEmpty(user.BankcomEmail))
            {
                return new AuthResponse
                {
                    UserCode = vm.Usercode,
                    NoEmail = true,
                    Success = true
                };
            }

            if (isFirstLogin)
            {
                return new AuthResponse
                {
                    FirstLogin = true,
                    UserCode = vm.Usercode,
                    Success = true
                };
            }

            if (isAuthenticated)
            {
                if (user.MustChangePW)
                {
                    return new AuthResponse
                    {
                        IsPasswordMustChange = true,
                        UserCode = vm.Usercode,
                        Success = true
                    };
                }
                else {
                    return new AuthResponse
                    {
                        IsAuthenticated = true,
                        FirstLogin = false,
                        Success = true,
                        Message = "Login Successfully",
                        Token = GenerateJWTToken(user)
                    };
                }
               
            }
            else
            {
                return new AuthResponse
                {
                    FirstLogin = false,
                    Success = false,
                    Message = "Invalid username or password"
                };
            }
            
        }
        catch (Exception)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred during login."
            };
        }
    }


    private string GenerateJWTToken(NGAC_USERINFO user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var userClaims = new List<Claim>
        {
        new Claim(ClaimTypes.NameIdentifier, user.ID),
        new Claim(ClaimTypes.Name, user.Name!),
        new Claim(ClaimTypes.Role, user.AccessType!),
        new Claim(ClaimTypes.Email, user.BankcomEmail!)

    };
        //if (!string.IsNullOrEmpty(user.BankcomEmail))
        //{
        //    userClaims.Add(new Claim(ClaimTypes.Email, user.BankcomEmail));
        //}

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddHours(int.Parse(_configuration["Jwt:TokenExpiryHours"])),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}
