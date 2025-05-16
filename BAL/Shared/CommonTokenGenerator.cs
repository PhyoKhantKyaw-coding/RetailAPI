using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace BAL.Shared;

public class CommonTokenGenerator(IConfiguration configuration)
{
    //public string Create(User user)
    //{
    //    string secretKey = configuration["Jwt:Secret"];
    //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

    //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    //    var tokenDescriptor = new SecurityTokenDescriptor
    //    {
    //        Subject = new ClaimsIdentity(new[]
    //        {
    //                new Claim(ClaimTypes.Email, user.Email)
    //            }),
    //        Expires = DateTime.UtcNow.AddMinutes(int.Parse(configuration["Jwt:ExpirationInMinutes"])),
    //        SigningCredentials = credentials,
    //        Issuer = configuration["Jwt:Issuer"],
    //        Audience = configuration["Jwt:Audience"],
    //    }; 

    //    var handler = new JsonWebTokenHandler();
    //    string token = handler.CreateToken(tokenDescriptor);
    //    return token;
    //}
    public string Create(User user, string userRole)
    {
        string secretKey = configuration["Jwt:Secret"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, userRole),
        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
        new Claim(ClaimTypes.Name,user.Name),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(configuration["Jwt:ExpirationInMinutes"])),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
        };

        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);
        return token;
    }

}

