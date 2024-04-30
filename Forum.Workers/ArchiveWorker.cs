// Copyright (C) TBC Bank. All Rights Reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;

namespace Forum.ArchiveWorker;

public class ArchiveWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _config;
    private readonly CrontabSchedule _schedule;
    private readonly ILogger<ArchiveWorker> _logger;
    private DateTime _nextRun;

    public ArchiveWorker(IServiceProvider serviceProvider, IConfiguration config, ILogger<ArchiveWorker> logger)
    {
        var cronExpression = config.GetValue<string>("Constants:CronExpressionForArchiveWorker");

        _serviceProvider = serviceProvider;
        _config = config;
        _logger = logger;
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
                _logger.LogInformation("Starting to execute Archive Work Run");

                await DoWork(stoppingToken);
                _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);

                _logger.LogInformation("Finished executing Archive Work Run");
            }
        }
    }
    private async Task DoWork(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        var archiveService = scope.ServiceProvider.GetRequiredService<ArchiveService>();

        var topics = await archiveService.GetTopicsWithLastestCommentAsync(stoppingToken);
        var daysToMoveToArchive = _config.GetValue<int>("Constants:DaysToMoveToArchive");
        

        foreach (var topic in topics)
        {
            var lastDate = topic.LatestComment?.CreatedAt ?? topic.ModifiedAt;
            if (lastDate.AddDays(daysToMoveToArchive) <= DateTime.UtcNow)
            {
                await archiveService.UpdateTopicStatusAsync(topic.TopicId, stoppingToken);
            }
        }
    }
}
