using Forum.Common.Mapping;
using Forum.Common.Middlewares;
using Forum.Common.Services;
using Forum.Persistance.Seed;
using Forum.Web.Infrastructure.Extensions.Middlewares;
using Forum.Web.Infrastructure.Extensions.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Full setup of serilog. We read log settings from appsettings.json
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddCustomMapping();

builder.Services.AddCustomValidation();

builder.Services.AddDbContextAndIdentity(builder.Configuration);
builder.Services.AddCommonServices();
builder.Services.AddCustomServices();
builder.Services.AddCustomHealthChecks(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opts =>
    {
        opts.LoginPath = "/Accounts/Login";
    });

var app = builder.Build();

app.UseSession();
app.UseGlobalErrorHandling();

app.UseHttpsRedirection();
app.UseConfiguredHealthCkecks(builder.Configuration);
app.UseConfiguredHealthChecksUI(builder.Configuration);
app.UseConfiguredStaticFiles(builder.Environment, builder.Configuration);
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Topic}/{action=Index}/{id?}");
});

app.Run();
