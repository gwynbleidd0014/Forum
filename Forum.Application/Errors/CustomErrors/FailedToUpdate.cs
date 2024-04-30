// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Errors.ErrorAbstractions;

namespace Forum.Application.Errors.CustomErrors;

public class FailedToUpdate : Exception, ICustomException
{
    public FailedToUpdate(string? message) : base(message)
    {
    }
}
