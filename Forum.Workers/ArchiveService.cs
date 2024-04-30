// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Topics.Admin;
using Forum.Application.Topics.Request;
using Forum.Application.Topics.Response;
using Forum.Domain.Enums;

namespace Forum.ArchiveWorker;

public class ArchiveService
{
    private readonly IAdminTopicService _topicService;

    public ArchiveService(IAdminTopicService topicService)
    {
        _topicService = topicService;
    }

    public async Task<List<TopicResponseModelWithLastestComment>> GetTopicsWithLastestCommentAsync(CancellationToken token)
    {
        return await _topicService.GetTopicsWithLatestCommentNoTrackingAsync(token);
    }

    public async Task UpdateTopicStatusAsync(int topicId, CancellationToken token)
    {
        var model = new TopicStatusUpdateModel { Status = TopicStatus.Inactive, Id = topicId };
        await _topicService.UpdateStatusAsync(model, token);
    }
}
