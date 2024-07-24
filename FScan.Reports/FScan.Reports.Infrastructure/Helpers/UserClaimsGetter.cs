using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using FScan.Reports.Application.Models.DTOs;


namespace FScan.Reports.Infrastructure.Helpers;

public class UserClaimsGetter
{

    private readonly IHttpContextAccessor _httpContext;

    public UserClaimsGetter(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }
    public UserClaims GetClaims()
    {
        var httpContext = _httpContext.HttpContext;
        var userClaims = new UserClaims();

        if (httpContext.User.Identity.IsAuthenticated)
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();
            var accessToken = authorizationHeader.Substring("Bearer ".Length).Trim();
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;
                var nameIdentifier = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var name = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var role = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                var email = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;


                userClaims = new UserClaims()
                {
                    ID = nameIdentifier,
                    Name = name,
                    AccessType = role,
                    Email = email
                   
                };
            }

        }
        return userClaims;
    }

   
}
