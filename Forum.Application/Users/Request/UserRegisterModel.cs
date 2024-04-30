// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Application.Users.Request;

public class UserRegisterModel
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
}
