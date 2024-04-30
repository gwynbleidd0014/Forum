// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Errors.ErrorAbstractions;

namespace Forum.Application.Errors.CustomErrors;

public class FileExstensionNotAllowed : Exception, ICustomException
{
    public FileExstensionNotAllowed(string? message) : base(message)
    {
    }
}
