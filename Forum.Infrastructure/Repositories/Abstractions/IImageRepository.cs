// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Images;

namespace Forum.Infrastructure.Repositories.Abstractions;

public interface IImageRepository
{
    Task<Image?> GetAsync(int id, CancellationToken token);
    Task AddAsync(Image model, CancellationToken token);
    Task RemoveAsync(int id, CancellationToken token);
    Task UpdateAsync(Image model, CancellationToken token);
}
