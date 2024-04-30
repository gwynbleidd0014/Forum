// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Errors.CustomErrors;
using Forum.Application.Resourses;
using Forum.Application.Users.Response;
using Forum.Domain.Users;
using Forum.Infrastructure.Repositories.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Forum.Application.Users.Admin;

public class AdminUserService : IAdminUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _config;
    public AdminUserService(IUserRepository userRepository, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _config = config;
    }

    public async Task<UsersWithTotalCountResponseModel> GetAllExceptAsync(int id, int skip, int take, CancellationToken token)
    {
        if (take < 1 || skip < 0)
            throw new Forbiden(ErrorMessages.NotAllowedPageSize);

        var result = await _userRepository.GetAllExceptAsync(id, skip, take, token);
        var pg = (skip / take) + 1;

        if (pg > 1 && result.Users.Count == 0)
            throw new NotFound(ErrorMessages.PageNotFound);

        return result.Adapt<UsersWithTotalCountResponseModel>();
    }

    public async Task<List<UserResponseModel>> GetBannedNoTrackingAsync(CancellationToken token)
    {
        var users = await _userRepository.GetBannedNoTrackingAsync(token);

        return users.Adapt<List<UserResponseModel>>();
    }

    public async Task BanUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id) ?? throw new NotFound(ErrorMessages.UserNotFound);

        user.IsBanned = true;
        user.BannedUntil = DateTime.UtcNow.AddDays(_config.GetValue<int>("Constants:BanTimeInDays"));

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new FailedToUpdate(ErrorMessages.FailedToBan);
    }

    public async Task UnBanUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id) ?? throw new NotFound(ErrorMessages.UserNotFound);

        user.IsBanned = false;
        user.BannedUntil = null;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new FailedToUpdate(ErrorMessages.FailedToBan);
    }

}
