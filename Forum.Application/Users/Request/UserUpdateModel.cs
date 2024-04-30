// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Application.Users.Request;

public class UserUpdateModel
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set;}
    public string? PhoneNumber { get; set; }
}
