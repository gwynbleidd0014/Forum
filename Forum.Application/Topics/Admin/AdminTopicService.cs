// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Errors.CustomErrors;
using Forum.Application.Resourses;
using Forum.Application.Topics.Request;
using Forum.Application.Topics.Response;
using Forum.Domain.Enums;
using Forum.Infrastructure.Repositories.Abstractions;
using Mapster;

namespace Forum.Application.Topics.Admin;

public class AdminTopicService : IAdminTopicService
{
    private readonly IAdminTopicRepository _topicRepository;

    public AdminTopicService(IAdminTopicRepository topicRepository)
    {
        _topicRepository = topicRepository;

    }
    public async Task<TopicAdminResponseModel> GetTopicByIdWithoutCommentsAsync(int id, CancellationToken token)
    {
        var topic = await _topicRepository.GetTopicByIdWithoutCommentsAsync(id, token);

        return topic == null ? throw new NotFound(ErrorMessages.TopicNotFound) : topic.Adapt<TopicAdminResponseModel>();
    }
    public async Task<TopicsWithTotalCountAdminResponseModel> GetUserTopicsWithCommentCountAsync(int userId, int skip, int take, CancellationToken token)
    {
        if (take < 1 || skip < 0)
            throw new Forbiden(ErrorMessages.NotAllowedPageSize);

        var result = await _topicRepository.GetUsersTopicsWithCommentCountAsync(userId, skip, take, token);
        var pg = (skip / take) + 1;

        if (pg > 1 && result.Topics.Count == 0)
            throw new NotFound(ErrorMessages.PageNotFound);

        return result.Adapt<TopicsWithTotalCountAdminResponseModel>();
    }

    public async Task<TopicsWithTotalCountAdminResponseModel> GetAllAsync(int skip, int take, CancellationToken token)
    {
        if (take < 1 || skip < 0)
            throw new Forbiden(ErrorMessages.NotAllowedPageSize);

        var result = await _topicRepository.GetAllAsync(skip, take, token);
        var pg = (skip / take) + 1;

        if (pg > 1 && result.Topics.Count == 0)
            throw new NotFound(ErrorMessages.PageNotFound);

        return result.Adapt<TopicsWithTotalCountAdminResponseModel>();
    }

    public async Task<TopicsWithTotalCountAdminResponseModel> GetArchivedAsync(int skip, int take, CancellationToken token)
    {
        if (take < 1 || skip < 0)
            throw new Forbiden(ErrorMessages.NotAllowedPageSize);

        var result = await _topicRepository.GetArchivedAsync(skip, take, token);
        var pg = (skip / take) + 1;

        if (pg > 1 && result.Topics.Count == 0)
            throw new NotFound(ErrorMessages.PageNotFound);

        return result.Adapt<TopicsWithTotalCountAdminResponseModel>();
    }

    public async Task<TopicAdminResponseModel> GetTopicByIdAsync(int id, CancellationToken token)
    {
        var topic = await _topicRepository.GetTopicByIdAsync(id, token);

        return topic == null ? throw new NotFound(ErrorMessages.TopicNotFound) : topic.Adapt<TopicAdminResponseModel>();
    }

    public async Task<List<TopicResponseModelWithLastestComment>> GetTopicsWithLatestCommentNoTrackingAsync(CancellationToken token)
    {
        var topics = await _topicRepository.GetTopicsWithLatestCommentNoTrackingAsync(token);
        return topics.Adapt<List<TopicResponseModelWithLastestComment>>();
    }

    public async Task UpdateStateAsync(TopicStateUpdateModel model, CancellationToken token)
    {
        if (model.State != TopicState.Show && model.State != TopicState.Hide)
            throw new NotFound(ErrorMessages.NoSuchTopicState);

        var topic = await _topicRepository.GetTopicByIdAsync(model.Id, token) ?? throw new NotFound(ErrorMessages.TopicNotFound);
        topic.State = (TopicState)model.State;

        await _topicRepository.UpdateAsync(topic, token);
    }

    public async Task UpdateStatusAsync(TopicStatusUpdateModel model, CancellationToken token)
    {
        if (model.Status != TopicStatus.Active && model.Status != TopicStatus.Inactive)
            throw new NotFound(ErrorMessages.NoSuchTopicStatus);

        var topic = await _topicRepository.GetTopicByIdAsync(model.Id, token) ?? throw new NotFound(ErrorMessages.TopicNotFound);
        topic.Status = (TopicStatus)model.Status;

        await _topicRepository.UpdateAsync(topic, token);
    }

}
