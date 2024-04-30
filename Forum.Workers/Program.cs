// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Comments;
using Forum.Application.Topics.Admin;
using Forum.Application.Users.Admin;
using Forum.ArchiveWorker;
using Forum.Common.Services;
using Forum.Infrastructure.Repositories.Abstractions;
using Forum.Infrastructure.Repositories.Implementations;
using Forum.Infrastructure.Repositories.Implementations.Repositories;
using Forum.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

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
    Log.Fatal(ex, ex.Message);
}

Console.ReadLine();

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContextAndIdentity(configuration, ServiceLifetime.Transient);
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IAdminUserService, AdminUserService>();
        services.AddTransient<BanService>();
        services.AddHostedService<BanWorker>();
        services.AddTransient<ICommentRepository, CommentRepository>();
        services.AddTransient<ICommentService, CommentService>();
        services.AddTransient<IAdminTopicRepository, AdminTopicRepository>();
        services.AddTransient<IAdminTopicService, AdminTopicService>();
        services.AddTransient<ArchiveService>();
        services.AddHostedService<ArchiveWorker>();
    })
    .UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services));

