// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Errors.ErrorAbstractions;

namespace Forum.Application.Errors.CustomErrors;

public class NotEnoughComments : Exception, ICustomException
{
    public NotEnoughComments(string? message) : base(message)
    {
    }
}
