// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Topics;

namespace Forum.Infrastructure.Repositories.Abstractions;

public interface IAdminTopicRepository
{
    Task<TopicsWithTotalCount> GetAllAsync(int skip, int take, CancellationToken token);
    Task<TopicsWithTotalCount> GetArchivedAsync(int skip, int take, CancellationToken token);
    Task<TopicsWithTotalCount> GetUsersTopicsWithCommentCountAsync(int userId, int skip, int take, CancellationToken token);
    Task<Topic?> GetTopicByIdAsync(int id, CancellationToken token);
    Task<List<TopicWithLatestComment>> GetTopicsWithLatestCommentNoTrackingAsync(CancellationToken token);
    Task UpdateAsync(Topic model, CancellationToken token);
    Task<Topic?> GetTopicByIdWithoutCommentsAsync(int id, CancellationToken token);

}
