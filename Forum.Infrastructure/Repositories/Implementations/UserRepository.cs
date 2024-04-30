// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Users;
using Forum.Infrastructure.Repositories.Abstractions;
using Forum.Infrastructure.Repositories.Implementations.Repositories;
using Forum.Persistance.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Repositories.Implementations;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> FindByNameWithImageAsync(string userName, CancellationToken token)
    {
        var user = await _dbSet
            .Include(x => x.Image)
            .SingleOrDefaultAsync(x => x.UserName == userName, token);

        return user;

    }

    public async Task<User?> FindByEmailWithImageAsync(string email, CancellationToken token)
    {
        var user = await _dbSet
            .Include(x => x.Image)
            .SingleOrDefaultAsync(x => x.Email == email, token);

        return user;
    }

    public async Task<User?> FindByIdWithImageAsync(int id, CancellationToken token)
    {
        var user = await _dbSet
            .Include(x => x.Image)
            .SingleOrDefaultAsync(x => x.Id == id, token);

        return user;
    }

    public async Task<List<User>> GetBannedNoTrackingAsync(CancellationToken token)
    {
        return await _dbSet.AsNoTracking().Where(x => x.IsBanned).ToListAsync(token);
    }

    public async Task<UsersWithTotalCount> GetAllExceptAsync(int id, int skip, int take, CancellationToken token)
    {
        IQueryable<User> users = _dbSet.Where(x => x.Id != id);

        var count = users.Count();
        var result = await users
            .OrderByDescending(x => x.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(token);

        return new UsersWithTotalCount { Users = result, TotalCount = count };

    }

    public async Task<bool> IsUniqueEmailAsync(string email, CancellationToken token)
    {
        return !(await _dbSet.AnyAsync(x => x.Email == email, token));
    }

    public async Task<bool> IsUniqueUserNameAsync(string username, CancellationToken token)
    {
        return !(await _dbSet.AnyAsync(x => x.UserName == username, token));
    }

    public async Task<bool> IsUniquePhoneNumberAsync(string phoneNumber, CancellationToken token)
    {
        return !(await _dbSet.AnyAsync(x => x.PhoneNumber == phoneNumber, token));
    }

    public async Task<int> GetUsersCommentCountAsync(int id, CancellationToken token)
    {
        return await _dbSet.Where(x => x.Id == id).Select(x => x.Comments.Count()).FirstOrDefaultAsync(token);
    }

    public async Task<bool> Exists(int id, CancellationToken token)
    {
        return await _dbSet.AnyAsync(x => x.Id == id, token);
    }
}
