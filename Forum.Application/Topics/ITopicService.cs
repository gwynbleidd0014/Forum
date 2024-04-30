// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Topics.Request;
using Forum.Application.Topics.Response;

namespace Forum.Application.Topics;

public interface ITopicService
{
    Task<TopicsWithTotalCountResponseModel> GetUserTopicsWithCommentCountAsync(int userId, int skip, int take, CancellationToken token);
    Task<TopicsWithTotalCountResponseModel> GetAllAsync(int skip, int take, CancellationToken token);
    Task<TopicsWithTotalCountResponseModel> GetArchivedAsync(int skip, int take, CancellationToken token);
    Task<TopicResponseModel> GetTopicByIdAsync(int id, CancellationToken token);
    Task CreateTopicAsync(TopicRequestModel model, int userId, CancellationToken token);
}
