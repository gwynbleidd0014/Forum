// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Accounts;
using Forum.Application.Enums;
using Forum.Application.Resourses;
using Forum.Application.Users.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterModel model, CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _accountService.CreateUserAsync(model, token);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }
            return View(model);
        }

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _accountService.LoginUserAsync(model);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("Login", ErrorMessages.UserNotFound);
            return View(model);
        }

        if (User.IsInRole(UserTypes.Admin.ToString()))
            return RedirectToAction("Topics", "Topic", new { area = "Admin" });

        return RedirectToAction(nameof(TopicController.Index), "Topic");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _accountService.SignOutAsync();
        return RedirectToAction(nameof(TopicController.Index), "Topic");
    }
}
