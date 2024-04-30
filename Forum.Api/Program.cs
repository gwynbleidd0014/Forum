// Copyright (C) TBC Bank. All Rights Reserved.
using Forum.Api.Infrastructure.Extensions.Middlewares;
using Forum.Api.Infrastructure.JwtAuth;
using Forum.Api.Infrastructure.ServiceConfiguration.Services;
using Forum.Api.Infrastructure.SwaggerAndVersioning;
using Forum.Common.Mapping;
using Forum.Common.Middlewares;
using Forum.Common.Services;
using Forum.Persistance.Seed;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Full setup of serilog. We read log settings from appsettings.json
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddConfiguredSwagger();
builder.Services.AddConfiguredVersioning();
builder.Services.AddCustomValidation();
builder.Services.AddCommonServices();
builder.Services.AddCustomServices();
builder.Services.AddCustomMapping();
builder.Services.AddDbContextAndIdentity(builder.Configuration);
builder.Services.AddJwtAuthentification(builder.Configuration);
builder.Services.AddCustomHealthChecks(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
        opts.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");
    });
}

app.UseGlobalErrorHandling();
app.Services.Seed();

app.UseConfiguredStaticFiles(builder.Environment, builder.Configuration);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseConfiguredHealthCkecks(builder.Configuration);
app.UseConfiguredHealthChecksUI(builder.Configuration);
app.UseAuthorization();

app.MapControllers();

app.Run();
