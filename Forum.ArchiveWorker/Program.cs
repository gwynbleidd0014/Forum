// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Comments;
using Forum.Application.Topics;
using Forum.ArchiveWorker;
using Forum.Infrastructure.Repositories.Abstractions;
using Forum.Infrastructure.Repositories.Implementations.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

try
{
    await CreateHostBuilder(args).Build().RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddTransient<ICommentRepository, CommentRepository>();
        services.AddTransient<ICommentService, CommentService>();
        services.AddTransient<ITopicRepository, TopicRepository>();
        services.AddTransient<ITopicService, TopicService>();
        services.AddTransient<ArchiveService>();
        services.AddHostedService<ArchiveWorker>();
    });
