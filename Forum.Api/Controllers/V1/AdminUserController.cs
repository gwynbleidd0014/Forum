// Copyright (C) TBC Bank. All Rights Reserved.

using System.Security.Claims;
using Asp.Versioning;
using Forum.Application.Users.Admin;
using Forum.Application.Users.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.V1;

[Authorize(Roles = "Admin")]
[ApiVersion("1.0")]
public class AdminUserController : CustomBaseController
{
    private readonly IAdminUserService _adminService;

    public AdminUserController(IAdminUserService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet]
    public async Task<UsersWithTotalCountResponseModel> GetAllAsync(CancellationToken token, [FromQuery] int pg = 1, [FromQuery] int pageSize = 10)
    {
        var recSkip = (pg - 1) * pageSize;
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _adminService.GetAllExceptAsync(userId, recSkip, pageSize, token);

        return result;
    }

    [HttpPost("ban/{id}")]
    public async Task BanUserAsync(string id, CancellationToken token)
    {
        await _adminService.BanUserAsync(id);
    }

    [HttpPost("unban/{id}")]
    public async Task UnBanUserAsync(string id, CancellationToken token)
    {
        await _adminService.UnBanUserAsync(id);
    }
}
