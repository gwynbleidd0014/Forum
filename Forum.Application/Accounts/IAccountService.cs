// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Users.Request;
using Microsoft.AspNetCore.Identity;

namespace Forum.Application.Accounts;

public interface IAccountService
{
    Task<IdentityResult> CreateUserAsync(UserRegisterModel model, CancellationToken token);
    Task<SignInResult> LoginUserAsync(UserLoginModel model);
    Task SignOutAsync();
    Task<List<string>> GetUserRolesAsync(string userName);
    Task RefreshSignInAsync(int id);
}
