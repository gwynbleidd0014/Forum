// Copyright (C) TBC Bank. All Rights Reserved.

using Microsoft.AspNetCore.Http;

namespace Forum.Application.Images;

public interface IImageService
{
    Task AddImageAsync(int userId, IFormFile file, CancellationToken token);
    Task<string> GetImagePathAsync(int userId, CancellationToken token);
    Task RemoveAsync(int userId, CancellationToken token);
    Task UpdateAsync(int userId, IFormFile file, CancellationToken token);
}
