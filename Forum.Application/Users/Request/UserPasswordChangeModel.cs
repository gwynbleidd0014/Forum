// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Application.Users.Request;

public class UserPasswordChangeModel
{
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmPassword { get; set;}
}
