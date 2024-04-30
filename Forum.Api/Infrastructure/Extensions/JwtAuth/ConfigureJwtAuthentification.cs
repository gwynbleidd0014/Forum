// Copyright (C) TBC Bank. All Rights Reserved.

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Forum.Api.Infrastructure.JwtAuth;

public static class ConfigureJwtAuthentification
{
    public static void AddJwtAuthentification(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(opts =>
        {
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config[$"{nameof(JwtConfig)}:{nameof(JwtConfig.Issuer)}"],
                    ValidateAudience = true,
                    ValidAudience = config[$"{nameof(JwtConfig)}:{nameof(JwtConfig.Audiance)}"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[$"{nameof(JwtConfig)}:{nameof(JwtConfig.Key)}"])),
                    ValidateLifetime = true,
                };
            });
    }
}
