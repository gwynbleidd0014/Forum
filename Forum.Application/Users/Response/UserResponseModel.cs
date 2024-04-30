// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Application.Users.Response;

public class UserResponseModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public int Id { get; set; }
    public bool IsBanned { get; set; }
    public DateTime? BannedUntil { get; set; }
    public string ImgPath { get; set; }
}
