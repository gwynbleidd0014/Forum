// Copyright (C) TBC Bank. All Rights Reserved.

using System.Security.Claims;
using Forum.Application.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers;

[Authorize]
public class ImageController : Controller
{
    private readonly IImageService _imageService;


    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken token)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        await _imageService.AddImageAsync(userId, file, token);

        return RedirectToAction(nameof(UserController.Profile), "User");
    }

    [HttpPost]
    public async Task<IActionResult> Remove(CancellationToken token)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        await _imageService.RemoveAsync(userId, token);

        return RedirectToAction(nameof(UserController.Profile), "User");
    }
}
