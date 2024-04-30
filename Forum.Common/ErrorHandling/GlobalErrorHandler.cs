// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Errors.ErrorAbstractions;
using Forum.Application.Resourses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Common.ErrorHandling;

public class GlobalErrorHandler : ProblemDetails
{
    public GlobalErrorHandler(HttpContext context, Exception ex)
    {
        Extensions["TraceId"] = context.TraceIdentifier;
        Instance = context.Request.Path;
        Title = ErrorMessages.SomethingWentWrong;
        if (ex is ICustomException)
            HandleCustomException((dynamic)ex);
        else HandleException();
    }

    private void HandleException()
    {
        Status = StatusCodes.Status500InternalServerError;
        Type = @"https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";
        Detail = ErrorMessages.SomethingWentWrong;
    }

    private void HandleCustomException(Exception ex)
    {
        Detail = ex.Message;
        if (ex is IBadRequest)
            HandleBadRequests();
        else if (ex is INotFound)
            HandleNotFounds();
        else if (ex is IConflict)
            HandleConflicts();
        else if (ex is IForbiden)
            HandleForbiddens();
        else
        {
            Status = StatusCodes.Status500InternalServerError;
            Type = @"https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";
        }

    }

    private void HandleForbiddens()
    {
        Status = StatusCodes.Status403Forbidden;
        Type = @"https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
    }

    private void HandleBadRequests()
    {
        Status = StatusCodes.Status400BadRequest;
        Type = @"https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
    }

    private void HandleNotFounds()
    {
        Status = StatusCodes.Status404NotFound;
        Type = @"https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
    }

    private void HandleConflicts()
    {
        Status = StatusCodes.Status409Conflict;
        Type = @"https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
    }
}
