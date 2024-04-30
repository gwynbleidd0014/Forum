// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Images;
using Forum.Infrastructure.Repositories.Abstractions;
using Forum.Infrastructure.Repositories.Implementations.Repositories;
using Forum.Persistance.DbContext;

namespace Forum.Infrastructure.Repositories.Implementations;

public class ImageRepository : BaseRepository<Image>, IImageRepository
{
    public ImageRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Image?> GetAsync(int id, CancellationToken token)
    {
        return await base.GetAsync(token, id);
    }
    public async new Task AddAsync(Image model, CancellationToken token)
    {
        await base.AddAsync(model, token);
    }

    public async Task RemoveAsync(int id, CancellationToken token)
    {
        await base.RemoveAsync(token, id);
    }

    public new async Task UpdateAsync(Image model, CancellationToken token)
    {
        await base.UpdateAsync(model, token);
    }
}
