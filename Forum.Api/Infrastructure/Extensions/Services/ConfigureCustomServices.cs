// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Api.Infrastructure.Middlewares;
using Forum.Common.Services;

namespace Forum.Api.Infrastructure.ServiceConfiguration.Services;

public static class ConfigureCustomServices
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<GlobalErrorHandlingMiddleware>();
    }

    public static void AddCustomHealthChecks(this IServiceCollection services, IConfiguration config)
    {
        var host = config.GetValue<string>("Constants:HostAddress");
        services.AddCommonHealthChecks(config)
            .AddUrlGroup(new Uri($"{host}/v1/topic"), "Topics Endpoint");
    }
}
