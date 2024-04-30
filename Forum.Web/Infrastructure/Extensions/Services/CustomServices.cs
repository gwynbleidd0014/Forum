// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Common.Services;
using Forum.Web.Infrastructure.Middlewares;

namespace Forum.Web.Infrastructure.Extensions.Services;

public static class CustomServices
{
    public static void AddCustomServices(this IServiceCollection builder)
    {
        builder.AddScoped<GlobalErrorHandlerMiddleware>();
    }

    public static void AddCustomHealthChecks(this IServiceCollection services, IConfiguration config)
    {
        var host = config.GetValue<string>("Constants:HostAddress");
        services.AddCommonHealthChecks(config)
            .AddUrlGroup(new Uri($"{host}/topic/index"), "Topics Endpoint");
    }
}
