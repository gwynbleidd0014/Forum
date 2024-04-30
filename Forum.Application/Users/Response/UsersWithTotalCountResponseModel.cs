// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Application.Users.Response;

public class UsersWithTotalCountResponseModel
{
    public List<UserResponseModel> Users { get; set; }
    public int TotalCount { get; set; }
}
