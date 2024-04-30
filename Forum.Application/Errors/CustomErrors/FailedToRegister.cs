// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Errors.ErrorAbstractions;

namespace Forum.Application.Errors.CustomErrors;

public class FailedToRegister : Exception, ICustomException
{
    public FailedToRegister(string? message) : base(message)
    {
    }
}
