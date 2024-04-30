// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Users;

namespace Forum.Infrastructure.Repositories.Abstractions;

public interface IUserRepository
{
    Task<User?> FindByNameWithImageAsync(string userName, CancellationToken token);
    Task<User?> FindByIdWithImageAsync(int id, CancellationToken token);
    Task<UsersWithTotalCount> GetAllExceptAsync(int id, int skip, int take, CancellationToken token);
    Task<List<User>> GetBannedNoTrackingAsync(CancellationToken token);
    Task<User?> FindByEmailWithImageAsync(string email, CancellationToken token);
    Task<bool> IsUniqueEmailAsync(string email, CancellationToken token);
    Task<bool> IsUniqueUserNameAsync(string username, CancellationToken token);
    Task<bool> IsUniquePhoneNumberAsync(string phoneNumber, CancellationToken token);
    Task<int> GetUsersCommentCountAsync(int id, CancellationToken token);
    Task<bool> Exists(int id, CancellationToken token);
}
