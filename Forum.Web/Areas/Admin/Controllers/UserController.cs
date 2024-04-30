// Copyright (C) TBC Bank. All Rights Reserved.

using System.Security.Claims;
using Forum.Application.Users.Admin;
using Forum.Common.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly IAdminUserService _adminService;
    private readonly IConfiguration _config;

    public UserController(IAdminUserService adminService, IConfiguration config)
    {
        _adminService = adminService;
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> Users(CancellationToken token, int pg = 1)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (pg < 1)
            pg = 1;

        var pageSize = _config.GetValue<int>("Constants:PageSize");
        var recSkip = (pg - 1) * pageSize;

        var result = await _adminService.GetAllExceptAsync(userId, recSkip, pageSize, token);

        var pager = new Pager(result.TotalCount, pg, pageSize);
        ViewBag.Pager = pager;

        return View(result.Users);
    }

    [HttpPost]
    public async Task<IActionResult> Ban(string id)
    {
        await _adminService.BanUserAsync(id);
        return RedirectToAction((nameof(Users)));
    }

    [HttpPost]
    public async Task<IActionResult> UnBan(string id)
    {
        await _adminService.UnBanUserAsync(id);
        return RedirectToAction((nameof(Users)));
    }
}
