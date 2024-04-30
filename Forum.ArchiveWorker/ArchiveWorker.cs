// Copyright (C) TBC Bank. All Rights Reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NCrontab;

namespace Forum.ArchiveWorker;

public class ArchiveWorker : BackgroundService
{
    private readonly ArchiveService _archiveService;
    private readonly IConfiguration _config;
    private readonly CrontabSchedule _schedule;
    private DateTime _nextRun;

    public ArchiveWorker(ArchiveService archiveService, IConfiguration config)
    {
        _archiveService = archiveService;
        _config = config;
        var cronExpression = config.GetValue<string>("Constants:CronExpressionForArchiveWorker");
        _schedule = CrontabSchedule.Parse(cronExpression, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
        _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            if (now > _nextRun)
            {
                await DoWork(stoppingToken);
                _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
            }
        }
    }
    private async Task DoWork(CancellationToken stoppingToken)
    {
        var topics = await _archiveService.GetTopicsWithLastestCommentAsync(stoppingToken);
        var daysToMoveToArchive = _config.GetValue<int>("Constants:DaysToMoveToArchive");

        foreach (var topic in topics)
        {
            if (topic.LatestComment?.CreatedAt.AddDays(daysToMoveToArchive) <= DateTime.Now)
            {
                await _archiveService.UpdateTopicStatusAsync(topic.TopicId, stoppingToken);
            }
        }
    }
}
