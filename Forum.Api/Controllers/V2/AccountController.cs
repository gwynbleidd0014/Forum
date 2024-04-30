// Copyright (C) TBC Bank. All Rights Reserved.

using Asp.Versioning;
using Forum.Api.Infrastructure.JwtAuth;
using Forum.Application.Accounts;
using Forum.Application.Errors.CustomErrors;
using Forum.Application.Resourses;
using Forum.Application.Users;
using Forum.Application.Users.Request;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.V2;

[ApiVersion("2.0")]
public class AccountController : CustomBaseController
{
    private readonly IAccountService _accountService;
    private readonly IUserService _userService;
    private readonly IConfiguration _config;

    public AccountController(IAccountService accountService, IUserService userService, IConfiguration config)
    {
        _accountService = accountService;
        _userService = userService;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<string> LoginAsync(UserLoginModel model)
    {
        var result = await _accountService.LoginUserAsync(model);
        if (!result.Succeeded)
            throw new NotFound(ErrorMessages.UserNotFound);

        var user = await _userService.FindByNameAsync(model.UserName!);
        var userRoles = await _accountService.GetUserRolesAsync(model.UserName!);

        return JwtHelper.GenerateToken(user, userRoles, _config);

    }
}
