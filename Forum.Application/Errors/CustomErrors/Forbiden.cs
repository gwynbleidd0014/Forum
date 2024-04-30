// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Errors.ErrorAbstractions;

namespace Forum.Application.Errors.CustomErrors;

public class Forbiden : Exception, ICustomException, IForbiden
{
    public Forbiden(string? message) : base(message)
    {
    }
}
