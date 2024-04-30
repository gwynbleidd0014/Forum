// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Errors.ErrorAbstractions;

namespace Forum.Application.Errors.CustomErrors;

public class NotFound : Exception, ICustomException, INotFound
{
    public NotFound(string msg) : base(msg)
    {
    }
}
