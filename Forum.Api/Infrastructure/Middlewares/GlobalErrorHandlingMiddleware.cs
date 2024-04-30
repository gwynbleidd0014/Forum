// Copyright (C) TBC Bank. All Rights Reserved.
using Forum.Common.ErrorHandling;

namespace Forum.Api.Infrastructure.Middlewares;

public class GlobalErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalErrorHandler> _logger;

    public GlobalErrorHandlingMiddleware(ILogger<GlobalErrorHandler> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var problem = new GlobalErrorHandler(context, ex);
            _logger.LogError(ex, "Error occured");
            context.Response.Clear();
            await context.Response.WriteAsJsonAsync(problem);

        }
    }
}
