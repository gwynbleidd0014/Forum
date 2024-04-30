// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Web.Infrastructure.Middlewares;

namespace Forum.Web.Infrastructure.Extensions.Middlewares;

public static class CustomMiddlewares
{
    public static void UseGlobalErrorHandling(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<GlobalErrorHandlerMiddleware>();
    }
}
