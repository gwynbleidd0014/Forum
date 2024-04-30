// Copyright (C) TBC Bank. All Rights Reserved.

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Forum.Application.Users.Response;
using Microsoft.IdentityModel.Tokens;

namespace Forum.Api.Infrastructure.JwtAuth;

public static class JwtHelper
{
    public static string GenerateToken(UserResponseModel model, List<string> userRoles, IConfiguration config)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[$"{nameof(JwtConfig)}:{nameof(JwtConfig.Key)}"]));
        var issuer = config[$"{nameof(JwtConfig)}:{nameof(JwtConfig.Issuer)}"];
        var audiance = config[$"{nameof(JwtConfig)}:{nameof(JwtConfig.Audiance)}"];
        var expDate = DateTime.UtcNow.AddMinutes(double.Parse(config[$"{nameof(JwtConfig)}:{nameof(JwtConfig.Exp)}"]));
        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, model.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Name, model.UserName),
            new Claim(ClaimTypes.NameIdentifier, model.Id.ToString())
        };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var token = new JwtSecurityToken(
            issuer,
            audiance,
            claims,
            expires: expDate,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }
}
