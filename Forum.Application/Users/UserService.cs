// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Errors.CustomErrors;
using Forum.Application.Resourses;
using Forum.Application.Users.Request;
using Forum.Application.Users.Response;
using Forum.Domain.Users;
using Forum.Infrastructure.Repositories.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Forum.Application.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _config;
    public UserService(IUserRepository userRepository, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
    }

    public async Task<UserResponseModel> FindByEmailAsync(string email, CancellationToken token)
    {
        var user = await _userRepository.FindByEmailWithImageAsync(email, token);

        return user == null ? throw new NotFound(ErrorMessages.UserNotFound) : user.Adapt<UserResponseModel>(); ;
    }

    public async Task<UserResponseModel> FindByNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return user.Adapt<UserResponseModel>();
    }

    public async Task<UserResponseModel> FindByIdAsync(int id, CancellationToken token)
    {
        var user = await _userRepository.FindByIdWithImageAsync(id, token);

        return user == null ? throw new NotFound(ErrorMessages.UserNotFound) : user.Adapt<UserResponseModel>(); ;
    }

    public async Task<bool> UserExists(int id, CancellationToken token)
    {
        return await _userRepository.Exists(id, token);
    }

    public async Task ChangePasswordAsync(string id, UserPasswordChangeModel model)
    {
        var user = await _userManager.FindByIdAsync(id);
        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword!, model.NewPassword!);
        if (!result.Succeeded)
            throw new FailedToUpdate(ErrorMessages.FaildToUpdate);

        await _signInManager.RefreshSignInAsync(user);
    }

    public async Task UpdateUserInfoAsync(UserUpdateModel model, string userId, CancellationToken token)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new NotFound(ErrorMessages.UserNotFound);
        await ValidateUniqueness(model, user, token);
        user = MapUser(user, model);

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new FailedToUpdate(ErrorMessages.FaildToUpdate);

        await _signInManager.RefreshSignInAsync(user);
    }

    public async Task RemovePhoneNumberAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new NotFound(ErrorMessages.UserNotFound);
        user.PhoneNumber = null;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new FailedToUpdate(ErrorMessages.FaildToUpdate);
    }

    public async Task<bool> IsAbleToCreateTopic(int userId, int validCount, CancellationToken token)
    {
        var count = await _userRepository.GetUsersCommentCountAsync(userId, token);

        return count >= validCount;
    }

    private static User MapUser(User des, UserUpdateModel src)
    {
        if (!string.IsNullOrEmpty(src.UserName) && des.UserName != src.UserName)
            des.UserName = src.UserName;
        if (!string.IsNullOrEmpty(src.Email) && des.Email != src.Email)
            des.Email = src.Email;
        if (!string.IsNullOrEmpty(src.PhoneNumber) && des.PhoneNumber != src.PhoneNumber)
            des.PhoneNumber = src.PhoneNumber;
        if (!string.IsNullOrEmpty(src.FirstName) && des.FirstName != src.FirstName)
            des.FirstName = src.FirstName;
        if (!string.IsNullOrEmpty(src.LastName) && des.LastName != src.LastName)
            des.LastName = src.LastName;

        return des;
    }

    private async Task ValidateUniqueness(UserUpdateModel model, User des, CancellationToken token)
    {
        if (model.Email != null && model.Email != des.Email && (await _userRepository.IsUniqueEmailAsync(model.Email, token) == false))
            throw new AlreadyExists(ErrorMessages.EmailExists);
        if (model.UserName != null && model.UserName != des.UserName && (await _userRepository.IsUniqueUserNameAsync(model.UserName, token) == false))
            throw new AlreadyExists(ErrorMessages.UserNameExists);
        if (model.PhoneNumber != null && model.PhoneNumber != des.PhoneNumber && (await _userRepository.IsUniquePhoneNumberAsync(model.PhoneNumber, token) == false))
            throw new AlreadyExists(ErrorMessages.PhoneNumberExists);
    }
}
