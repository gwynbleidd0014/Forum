// Copyright (C) TBC Bank. All Rights Reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;

namespace Forum.Workers;

public class BanWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CrontabSchedule _schedule;
    private readonly ILogger<BanWorker> _logger;
    private DateTime _nextRun;

    public BanWorker(IConfiguration config, ILogger<BanWorker> logger, IServiceProvider serviceProvider)
    {
        var cronExpression = config.GetValue<string>("Constants:CronExpressionForBanWorker");

        _logger = logger;
        _serviceProvider = serviceProvider;
        _schedule = CrontabSchedule.Parse(cronExpression, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
        _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            if (now > _nextRun)
            {
                _logger.LogInformation("Starting to execute Ban Work Run");

                await DoWork(stoppingToken);
                _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);

                _logger.LogInformation("Finished executing Ban Work Run");
            }
        }
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        var banService = scope.ServiceProvider.GetRequiredService<BanService>();

        var users = await banService.GetBannedAsync(stoppingToken);

        foreach (var user in users)
        {
            if (user.BannedUntil <= DateTime.UtcNow)
            {
                await banService.UnBanUser(user.Id.ToString());
            }
        }
    }
}
