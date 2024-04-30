// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Users.Response;

namespace Forum.Application.Users.Admin;

public interface IAdminUserService
{
    Task<UsersWithTotalCountResponseModel> GetAllExceptAsync(int id, int skip, int take, CancellationToken token);
    Task<List<UserResponseModel>> GetBannedNoTrackingAsync(CancellationToken token);
    Task BanUserAsync(string id);
    Task UnBanUserAsync(string id);
}
