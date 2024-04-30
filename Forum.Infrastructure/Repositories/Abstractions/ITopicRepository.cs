// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Topics;

namespace Forum.Infrastructure.Repositories.Abstractions;

public interface ITopicRepository
{
    Task<TopicsWithTotalCount> GetAllAsync(int skip, int take, CancellationToken token);
    Task<TopicsWithTotalCount> GetArchivedAsync(int skip, int take, CancellationToken token);
    Task<TopicsWithTotalCount> GetUsersTopicsWithCommentCountAsync(int userId, int skip, int take, CancellationToken token);
    Task<Topic?> GetTopicByIdAsync(int id, CancellationToken token);
    Task CreateTopicAsync(Topic topic, CancellationToken token);
}
