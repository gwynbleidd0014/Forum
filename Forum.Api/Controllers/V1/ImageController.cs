// Copyright (C) TBC Bank. All Rights Reserved.

using System.Security.Claims;
using Forum.Application.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.V1;
[Authorize]
public class ImageController : CustomBaseController
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost]
    public async Task AddImageAsync(IFormFile file, CancellationToken token)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        await _imageService.AddImageAsync(userId, file, token);
    }

    [HttpGet]
    public async Task<string> GetImageUrlAsync(CancellationToken token)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var imagePath = await _imageService.GetImagePathAsync(userId, token);
        var imgUrl = $"{Request.Scheme}://{Request.Host}{imagePath}";

        return imgUrl;
    }

    [HttpPut]
    public async Task UpdateImageAsync(IFormFile file, CancellationToken token)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        await _imageService.UpdateAsync(userId, file, token);
    }

    [HttpDelete]
    public async Task RemoveImageAsync(CancellationToken token)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        await _imageService.RemoveAsync(userId, token);
    }
}
