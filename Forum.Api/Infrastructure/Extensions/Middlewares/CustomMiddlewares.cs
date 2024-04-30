// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Api.Infrastructure.Middlewares;

namespace Forum.Api.Infrastructure.Extensions.Middlewares;

public static class CustomMiddlewares
{
    public static void UseGlobalErrorHandling(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<GlobalErrorHandlingMiddleware>();
    }
}
