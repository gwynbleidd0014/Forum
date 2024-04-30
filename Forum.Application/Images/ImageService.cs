// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Errors.CustomErrors;
using Forum.Application.Images.Request;
using Forum.Application.Resourses;
using Forum.Domain.Images;
using Forum.Infrastructure.Repositories.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Forum.Application.Images;

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;

    private readonly IConfiguration _config;
    public ImageService(IImageRepository imageRepository, IConfiguration config)
    {
        _imageRepository = imageRepository;
        _config = config;
    }

    public async Task AddImageAsync(int userId, IFormFile file, CancellationToken token)
    {
        var img = await _imageRepository.GetAsync(userId, token);
        if (img != null)
        {
            await UpdateAsync(userId, file, token);
        }
        else
        {
            var (path, absolutePath) = await SaveImage(file, token);
            var newImg = new ImageCreateModel() { UserId = userId, Path = path, AbsolutePath = absolutePath };
            await _imageRepository.AddAsync(newImg.Adapt<Image>(), token);
        }
    }
    public async Task<string> GetImagePathAsync(int userId, CancellationToken token)
    {
        var img = await _imageRepository.GetAsync(userId, token) ?? throw new NotFound(ErrorMessages.ImageNotFound);

        return img.Path;
    }

    public async Task RemoveAsync(int userId, CancellationToken token)
    {
        var img = await _imageRepository.GetAsync(userId, token) ?? throw new NotFound(ErrorMessages.ImageNotFound);
        RemoveImage(img.AbsolutePath);
        await _imageRepository.RemoveAsync(img.Id, token);
    }

    public async Task UpdateAsync(int userId, IFormFile file, CancellationToken token)
    {
        var img = await _imageRepository.GetAsync(userId, token) ?? throw new NotFound(ErrorMessages.ImageNotFound);
        RemoveImage(img.AbsolutePath);
        var (path, absolutePath) = await SaveImage(file, token);
        img.Path = path;
        img.AbsolutePath = absolutePath;
        await _imageRepository.UpdateAsync(img, token);
    }

    private async Task<(string, string)> SaveImage(IFormFile file, CancellationToken token)
    {

        var path = _config.GetValue<string>("Constants:UploadsFolderPath");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var ext = Path.GetExtension(file.FileName).ToLower();
        var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };

        if (!allowedExtensions.Contains(ext))
            throw new FileExstensionNotAllowed(string.Format(ErrorMessages.FileExstensionNotAllowed, string.Join(", ", allowedExtensions)));

        var uniqueString = Guid.NewGuid().ToString();
        var fileName = uniqueString + ext;
        var filePath = Path.Combine(path, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, token);
            await stream.FlushAsync(token);
        }

        return (string.Format("/{0}/{1}", _config.GetValue<string>("Constants:ResourcePath"), fileName), filePath);
    }

    private static void RemoveImage(string path)
    {
        if (!File.Exists(path))
            throw new NotFound(ErrorMessages.ImageNotFound);

        File.Delete(path);
    }
}
