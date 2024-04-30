// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Errors.ErrorAbstractions;

namespace Forum.Application.Errors.CustomErrors;

public class AlreadyExists : Exception, ICustomException, IConflict
{
    public AlreadyExists(string? message) : base(message)
    {
    }
}
