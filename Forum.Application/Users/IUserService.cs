// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Users.Request;
using Forum.Application.Users.Response;

namespace Forum.Application.Users;

public interface IUserService
{
    Task<UserResponseModel> FindByIdAsync(int id, CancellationToken token);
    Task<UserResponseModel> FindByEmailAsync(string email, CancellationToken token);
    Task<UserResponseModel> FindByNameAsync(string userName);
    Task ChangePasswordAsync(string id, UserPasswordChangeModel model);
    Task UpdateUserInfoAsync(UserUpdateModel model, string userId, CancellationToken token);
    Task RemovePhoneNumberAsync(string userId);
    Task<bool> IsAbleToCreateTopic(int userId, int validCount, CancellationToken token);
    Task<bool> UserExists(int id, CancellationToken token);
}
