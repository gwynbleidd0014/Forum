// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Enums;
using Forum.Application.Errors.CustomErrors;
using Forum.Application.Resourses;
using Forum.Application.Users.Request;
using Forum.Domain.Users;
using Forum.Infrastructure.Repositories.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Forum.Application.Accounts;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IUserRepository _userRepository;
    public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IUserRepository userRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
    }

    public async Task<IdentityResult> CreateUserAsync(UserRegisterModel model, CancellationToken token)
    {
        var user = model.Adapt<User>();
        if (!(await _userRepository.IsUniqueUserNameAsync(user.UserName, token)))
            throw new AlreadyExists(ErrorMessages.UserNameExists);

        if (!(await _userRepository.IsUniqueEmailAsync(user.Email, token)))
            throw new AlreadyExists(ErrorMessages.EmailExists);

        var result = await _userManager.CreateAsync(user, model.Password!);

        if (result.Succeeded)
            await _userManager.AddToRoleAsync(user, UserTypes.User.ToString());

        return result;
    }

    public async Task<SignInResult> LoginUserAsync(UserLoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName!) ?? throw new NotFound(ErrorMessages.UserNameNotFound);

        if (user.IsBanned == true)
            throw new Forbiden(ErrorMessages.UserIsBanned);

        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

        return result;
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<List<string>> GetUserRolesAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        var userRoles = await _userManager.GetRolesAsync(user);

        return userRoles.Adapt<List<string>>();
    }

    public async Task RefreshSignInAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString()) ?? throw new NotFound(ErrorMessages.UserNotFound);

        await _signInManager.RefreshSignInAsync(user);
    }
}
