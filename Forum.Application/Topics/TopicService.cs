// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Errors.CustomErrors;
using Forum.Application.Resourses;
using Forum.Application.Topics.Request;
using Forum.Application.Topics.Response;
using Forum.Application.Users;
using Forum.Domain.Topics;
using Forum.Infrastructure.Repositories.Abstractions;
using Mapster;
using Microsoft.Extensions.Configuration;

namespace Forum.Application.Topics;

public class TopicService : ITopicService
{
    private readonly ITopicRepository _topicRepository;
    private readonly IUserService _userService;
    private readonly IConfiguration _config;

    public TopicService(ITopicRepository topicRepository, IUserService userService, IConfiguration config)
    {
        _topicRepository = topicRepository;
        _userService = userService;
        _config = config;
    }

    public async Task<TopicsWithTotalCountResponseModel> GetUserTopicsWithCommentCountAsync(int userId, int skip, int take, CancellationToken token)
    {
        if (take < 1 || skip < 0)
            throw new Forbiden(ErrorMessages.NotAllowedPageSize);

        var result = await _topicRepository.GetUsersTopicsWithCommentCountAsync(userId, skip, take, token);
        var pg = skip / take + 1;

        if (pg > 1 && result.Topics.Count == 0)
            throw new NotFound(ErrorMessages.PageNotFound);

        return result.Adapt<TopicsWithTotalCountResponseModel>();
    }

    public async Task<TopicsWithTotalCountResponseModel> GetAllAsync(int skip, int take, CancellationToken token)
    {
        if (take < 1 || skip < 0)
            throw new Forbiden(ErrorMessages.NotAllowedPageSize);

        var result = await _topicRepository.GetAllAsync(skip, take, token);
        var pg = skip / take + 1;

        if (pg > 1 && result.Topics.Count == 0)
            throw new NotFound(ErrorMessages.PageNotFound);

        return result.Adapt<TopicsWithTotalCountResponseModel>();
    }

    public async Task<TopicsWithTotalCountResponseModel> GetArchivedAsync(int skip, int take, CancellationToken token)
    {
        if (take < 1 || skip < 0)
            throw new Forbiden(ErrorMessages.NotAllowedPageSize);

        var result = await _topicRepository.GetArchivedAsync(skip, take, token);
        var pg = skip / take + 1;

        if (pg > 1 && result.Topics.Count == 0)
            throw new NotFound(ErrorMessages.PageNotFound);

        return result.Adapt<TopicsWithTotalCountResponseModel>();
    }

    public async Task<TopicResponseModel> GetTopicByIdAsync(int id, CancellationToken token)
    {
        var topic = await _topicRepository.GetTopicByIdAsync(id, token);

        return topic == null ? throw new NotFound(ErrorMessages.TopicNotFound) : topic.Adapt<TopicResponseModel>();
    }

    public async Task CreateTopicAsync(TopicRequestModel model, int userId, CancellationToken token)
    {
        if (!(await _userService.UserExists(userId, token)))
            throw new NotFound(ErrorMessages.UserNotFound);

        var topic = model.Adapt<TopicCreateModel>();
        topic.UserId = userId;

        var validCount = int.Parse(_config["Constants:ValidCommentCount"]);
        if (!(await _userService.IsAbleToCreateTopic(topic.UserId, validCount, token)))
            throw new Forbiden(string.Format(ErrorMessages.NotEnoughComments, validCount));

        await _topicRepository.CreateTopicAsync(topic.Adapt<Topic>(), token);
    }
}
