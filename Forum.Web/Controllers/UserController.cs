// Copyright (C) TBC Bank. All Rights Reserved.

using System.Security.Claims;
using Forum.Application.Users;
using Forum.Application.Users.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IConfiguration _config;
    public UserController(IUserService userService, IConfiguration config)
    {
        _userService = userService;
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> Profile(CancellationToken token)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userService.FindByIdAsync(int.Parse(userId), token);

        return View(user);
    }
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> UserProfileByEmail(string email, CancellationToken token)
    {
        var user = await _userService.FindByEmailAsync(email, token);

        return RedirectToAction(nameof(UserProfile), new { id = user.Id });
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> UserProfile(int id, CancellationToken token)
    {
        var user = await _userService.FindByIdAsync(id, token);

        return View(user);
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(UserPasswordChangeModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (User.Identity?.Name == null)
            return RedirectToAction(nameof(AccountController.Login), "Account");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _userService.ChangePasswordAsync(userId, model);

        return RedirectToAction(nameof(Profile));
    }

    [HttpGet]
    public IActionResult Update()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Update(UserUpdateModel model, CancellationToken token)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _userService.UpdateUserInfoAsync(model, userId, token);

        return RedirectToAction(nameof(Profile));
    }

    [HttpGet]
    public async Task<IActionResult> RemovePhoneNumber()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _userService.RemovePhoneNumberAsync(userId);

        return RedirectToAction(nameof(Update));
    }
}
