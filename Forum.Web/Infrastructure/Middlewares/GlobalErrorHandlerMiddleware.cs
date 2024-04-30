// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Common.ErrorHandling;

namespace Forum.Web.Infrastructure.Middlewares;

public class GlobalErrorHandlerMiddleware : IMiddleware
{
    private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;
    public GlobalErrorHandlerMiddleware(ILogger<GlobalErrorHandlerMiddleware> logger)
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

            _logger.LogError(ex, "Error occured");
            var problem = new GlobalErrorHandler(context, ex);
            context.Session.SetString("ErrorMessage", problem.Detail!);
            context.Response.Redirect("/Home/Error");
        }
    }
}
