using Forum.Domain.Comments;
using Forum.Domain.Images;
using Forum.Domain.Interfaces;
using Forum.Domain.Topics;
using Forum.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forum.Persistance.DbContext;

public class AppDbContext : IdentityDbContext<User, Role, int>
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Topic>? Topics { get; set; }
    public DbSet<Comment>? Comments { get; set; }
    public DbSet<Image>? Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entriesAddedOrModified = ChangeTracker.Entries()
            .Where(x => x.Entity is IEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        var entriesDeleted = ChangeTracker.Entries()
            .Where(x => x.Entity is ISoftDelete && x.State == EntityState.Deleted);

        foreach (var entry in entriesDeleted)
        {
            var entity = (ISoftDelete)entry.Entity;
            entity.IsDeleted = true;
            entry.State = EntityState.Modified;
        }

        foreach (var entry in entriesAddedOrModified)
        {
            var entity = (IEntity)entry.Entity;
            entity.ModifiedAt = DateTime.UtcNow;
            if (entry.State == EntityState.Added)
                entity.CreatedAt = DateTime.UtcNow;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
