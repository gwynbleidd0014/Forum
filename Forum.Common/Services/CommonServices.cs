// Copyright (C) TBC Bank. All Rights Reserved.

using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Forum.Application.Accounts;
using Forum.Application.Comments;
using Forum.Application.Images;
using Forum.Application.Topics;
using Forum.Application.Topics.Admin;
using Forum.Application.Users;
using Forum.Application.Users.Admin;
using Forum.Domain.Users;
using Forum.Infrastructure.Repositories.Abstractions;
using Forum.Infrastructure.Repositories.Implementations;
using Forum.Infrastructure.Repositories.Implementations.Repositories;
using Forum.Persistance.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Common.Services;

public static class CommonServices
{
    public static void AddCommonServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAccountService, AccountService>();

        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICommentService, CommentService>();

        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IImageService, ImageService>();

        services.AddScoped<IAdminTopicRepository, AdminTopicRepository>();
        services.AddScoped<IAdminTopicService, AdminTopicService>();

        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<ITopicService, TopicService>();

        services.AddScoped<IAdminUserService, AdminUserService>();
    }

    public static void AddCustomValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public static void AddDbContextAndIdentity(this IServiceCollection services, IConfiguration configuration, ServiceLifetime contextLifeTime = ServiceLifetime.Scoped)
    {
        services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), contextLifetime: contextLifeTime);
        services.AddIdentity<User, Role>(opts =>
        {
            opts.Password.RequireDigit = true;
        })
           .AddEntityFrameworkStores<AppDbContext>();
    }

    public static IHealthChecksBuilder AddCommonHealthChecks(this IServiceCollection services, IConfiguration config)
    {
        var health = services.AddHealthChecks()
        .AddSqlServer(config.GetConnectionString("DefaultConnection"));
        services.AddHealthChecksUI(opts =>
        {
            opts.SetEvaluationTimeInSeconds(10);
            opts.SetApiMaxActiveRequests(1);
            opts.AddHealthCheckEndpoint("Feedback", config.GetValue<string>("Constants:HealthCheckPath"));
        })
            .AddInMemoryStorage();

        return health;
    }
}
