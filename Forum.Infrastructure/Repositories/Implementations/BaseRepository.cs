using System.Linq.Expressions;
using Forum.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Repositories.Implementations.Repositories;

public class BaseRepository<T> where T : class
{
    protected readonly IdentityDbContext<User, Role, int> _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(IdentityDbContext<User, Role, int> context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    protected async Task<T?> GetAsync(CancellationToken token, params object[] key)
    {
        return await _dbSet.FindAsync(key, token);
    }

    protected async Task AddAsync(T entity, CancellationToken token)
    {
        await _dbSet.AddAsync(entity, token);
        await _context.SaveChangesAsync(token);
    }

    protected async Task UpdateAsync(T entity, CancellationToken token)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(token);
    }

    protected async Task RemoveAsync(CancellationToken token, params object[] key)
    {
        var comment = await _dbSet.FindAsync(key, token);
        if (comment == null)
            return;

        _dbSet.Remove(comment);
        await _context.SaveChangesAsync(token);
    }
}
