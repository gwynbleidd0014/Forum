// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Enums;
using Forum.Domain.Comments;
using Forum.Domain.Enums;
using Forum.Domain.Topics;
using Forum.Domain.Users;
using Forum.Persistance.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Persistance.Seed;

public static class ConfigureSeeding
{
    public static void Seed(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        Migrate(dbContext);
        SeedRoles(roleManager).GetAwaiter().GetResult();
        SeedAdminUsers(userManager).GetAwaiter().GetResult();
        SeedRegularUsers(userManager).GetAwaiter().GetResult();
        SeedTopics(dbContext, userManager).GetAwaiter().GetResult();
        SeedComments(dbContext, userManager).GetAwaiter().GetResult();
    }

    private static void Migrate(AppDbContext context)
    {
        context.Database.Migrate();
    }
    private static async Task SeedRoles(RoleManager<Role> roleManager)
    {
        var roles = new List<Role>()
        {
            new() { Name = UserTypes.Admin.ToString()},
            new() { Name = UserTypes.User.ToString()}
        };

        foreach (var role in roles)
        {
            if (await roleManager.FindByNameAsync(role.Name) == null)
            {
                await roleManager.CreateAsync(role);
            }
        }
    }

    private static async Task SeedAdminUsers(UserManager<User> userManager)
    {
        var users = new List<User>()
        {
            new() { FirstName = "Anomander", LastName = "Purerake", Email = "anomander@gmail.com", PhoneNumber = "597000000", UserName = "anomander01", PasswordHash = "Anomander12#"},
            new() { FirstName = "Cotillion", LastName = "Dancer", Email = "cotillion@gmail.com", PhoneNumber = "597111111", UserName = "cotillion01", PasswordHash = "Cotillion12#"}
        };

        foreach (var user in users)
        {
            if (await userManager.FindByNameAsync(user.UserName) == null)
            {
                await userManager.CreateAsync(user, user.PasswordHash);
                await userManager.AddToRoleAsync(user, UserTypes.Admin.ToString());

            }
        }
    }

    private static async Task SeedRegularUsers(UserManager<User> userManager)
    {
        var users = new List<User>()
        {
            new() { FirstName = "Giorno", LastName = "Giovanna", Email = "giorno@gmail.com", PhoneNumber = "595000000", UserName = "giorno01", PasswordHash = "Giorno12#"},
            new() { FirstName = "Jotaro", LastName = "Kudjo", Email = "jotaro@gmail.com", PhoneNumber = "595111111", UserName = "jotaro01", PasswordHash = "Jotaro12#"}
        };

        foreach (var user in users)
        {
            if (await userManager.FindByNameAsync(user.UserName) == null)
            {
                await userManager.CreateAsync(user, user.PasswordHash);
                await userManager.AddToRoleAsync(user, UserTypes.User.ToString());
            }
        }
    }

    private static async Task SeedTopics(AppDbContext context, UserManager<User> userManager)
    {
        var userOne = await userManager.FindByNameAsync("giorno01");
        var userTwo = await userManager.FindByNameAsync("jotaro01");
        var topics = new List<Topic>()
        {
            new()
            {
                Name = "Building Forum App",
                Text = "I've Encounterd problem of building forum app, can anyone give any sugestions?",
                State = TopicState.Show,
                Status = TopicStatus.Active,
                UserId = userOne.Id,
            },
            new()
            {
                Name = "Where to start with Cosmere",
                Text = "I want to start reading branodn sandersons Cosemre books, where should I start, should I follow chronological order of books or is there any better path to take?",
                State = TopicState.Show,
                Status = TopicStatus.Active,
                UserId = userOne.Id
            },
            new()
            {
                Name = "What is Lorem Ipsum?",
                Text = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum",
                State = TopicState.Show,
                Status = TopicStatus.Active,
                UserId = userTwo.Id
            },
            new()
            {
                Name = "Why do we use it?",
                Text = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                State = TopicState.Show,
                Status = TopicStatus.Active,
                UserId = userTwo.Id
            }
        };

        foreach (var topic in topics)
        {

            if (await context.Topics!.FirstOrDefaultAsync(x => x.Name == topic.Name) == null)
            {
                await context.Topics!.AddAsync(topic);
                await context.SaveChangesAsync();
            }
        }
    }

    private static async Task SeedComments(AppDbContext context, UserManager<User> userManager)
    {
        var topicOne = await context.Topics?.SingleOrDefaultAsync(x => x.Name == "Building Forum App")!;
        var topicTwo = await context.Topics?.SingleOrDefaultAsync(x => x.Name == "What is Lorem Ipsum?")!;
        var userOne = await userManager.FindByNameAsync("giorno01");
        var userTwo = await userManager.FindByNameAsync("jotaro01");

        var comments = new List<Comment>
        {
            new()
            {
                UserId = userOne.Id,
                Text = "If you want to build something just strat doing it",
                TopicId = topicOne!.Id,
            },
            new()
            {
                UserId = userTwo.Id,
                Text = "If you want to build something just strat doing it",
                TopicId = topicTwo!.Id,
            }
        };

        foreach (var comment in comments)
        {

            if (await context.Comments!.FirstOrDefaultAsync(x => x.Text == comment.Text) == null)
            {
                await context.Comments!.AddAsync(comment);
                await context.SaveChangesAsync();
            }
        }
    }
}
