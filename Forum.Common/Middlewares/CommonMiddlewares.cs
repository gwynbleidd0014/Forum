// Copyright (C) TBC Bank. All Rights Reserved.

using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Forum.Common.Middlewares;

public static class CommonMiddlewares
{
    public static void UseConfiguredHealthChecksUI(this IApplicationBuilder builder, IConfiguration config)
    {
        builder.UseHealthChecksUI(opts =>
        {
            opts.UIPath = config.GetValue<string>("Constants:HealthCkeckUiPath");
        });
    }

    public static void UseConfiguredHealthCkecks(this IEndpointRouteBuilder builder, IConfiguration config)
    {
        builder.MapHealthChecks(config.GetValue<string>("Constants:HealthCheckPath"), new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }

    public static void UseConfiguredStaticFiles(this IApplicationBuilder builder, IWebHostEnvironment environment, IConfiguration config)
    {
        builder.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(config.GetValue<string>("Constants:UploadsFolderPath")),
            RequestPath = string.Format("/{0}", config.GetValue<string>("Constants:ResourcePath"))
        });
    }
}
