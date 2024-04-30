// Copyright (C) TBC Bank. All Rights Reserved.

using System.Security.Claims;
using Asp.Versioning;
using Forum.Api.Models.User;
using Forum.Application.Users;
using Forum.Application.Users.Request;
using Forum.Application.Users.Response;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public class UserController : CustomBaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<UserResponseModel> GetUserByIdAsync(CancellationToken token)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return await _userService.FindByIdAsync(userId, token);
    }

    [HttpGet("email")]
    public async Task<UserApiResponseModel> GetUserByEmailAsync(string email, CancellationToken token)
    {
        var user = await _userService.FindByEmailAsync(email, token);

        return user.Adapt<UserApiResponseModel>();
    }

    [HttpPatch]
    public async Task ChangePasswordAsync(UserPasswordChangeModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _userService.ChangePasswordAsync(userId, model);
    }

    [HttpPut]
    public async Task UpdateUserInfoAsync(UserUpdateModel model, CancellationToken token)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _userService.UpdateUserInfoAsync(model, userId, token);
    }

    [HttpDelete]
    public async Task RemovePhoneNumberAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _userService.RemovePhoneNumberAsync(userId);
    }
}
