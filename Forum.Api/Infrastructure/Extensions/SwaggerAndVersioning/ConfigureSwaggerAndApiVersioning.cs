// Copyright (C) TBC Bank. All Rights Reserved.

using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace Forum.Api.Infrastructure.SwaggerAndVersioning;

public static class ConfigureSwaggerAndApiVersioning
{
    public static void AddConfiguredSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opts =>
        {
            opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Description = "Authorization for Forum Api"
            });

            opts.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            opts.SwaggerDoc("v1", new OpenApiInfo() { Title = "Forum Api", Version = "1.0" });
            opts.SwaggerDoc("v2", new OpenApiInfo() { Title = "Forum Api", Version = "2.0" });
        });
    }

    public static void AddConfiguredVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opts =>
        {
            opts.ApiVersionReader = new UrlSegmentApiVersionReader();
            opts.DefaultApiVersion = new(1, 0);
            opts.AssumeDefaultVersionWhenUnspecified = true;
        })
            .AddApiExplorer(opts =>
            {
                opts.GroupNameFormat = "'v'VVV";
                opts.SubstituteApiVersionInUrl = true;
            });
    }
}
