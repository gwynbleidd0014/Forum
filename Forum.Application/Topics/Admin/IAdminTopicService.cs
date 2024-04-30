// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Topics.Request;
using Forum.Application.Topics.Response;

namespace Forum.Application.Topics.Admin;

public interface IAdminTopicService
{
    Task<TopicAdminResponseModel> GetTopicByIdWithoutCommentsAsync(int id, CancellationToken token);
    Task<TopicsWithTotalCountAdminResponseModel> GetUserTopicsWithCommentCountAsync(int userId, int skip, int take, CancellationToken token);
    Task<TopicsWithTotalCountAdminResponseModel> GetAllAsync(int skip, int take, CancellationToken token);
    Task<TopicsWithTotalCountAdminResponseModel> GetArchivedAsync(int skip, int take, CancellationToken token);
    Task<TopicAdminResponseModel> GetTopicByIdAsync(int id, CancellationToken token);
    Task<List<TopicResponseModelWithLastestComment>> GetTopicsWithLatestCommentNoTrackingAsync(CancellationToken token);
    Task UpdateStateAsync(TopicStateUpdateModel model, CancellationToken token);
    Task UpdateStatusAsync(TopicStatusUpdateModel model, CancellationToken token);
}
