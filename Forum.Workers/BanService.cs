// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Users.Admin;
using Forum.Application.Users.Response;

namespace Forum.Workers;

public class BanService
{
    private readonly IAdminUserService _userService;

    public BanService(IAdminUserService userService)
    {
        _userService = userService;
    }

    public async Task UnBanUser(string id)
    {
        await _userService.UnBanUserAsync(id);
    }

    public async Task<List<UserResponseModel>> GetBannedAsync(CancellationToken token)
    {
        return await _userService.GetBannedNoTrackingAsync(token);
    }
}
