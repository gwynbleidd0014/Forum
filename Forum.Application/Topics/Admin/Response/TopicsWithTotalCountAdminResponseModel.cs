// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Application.Topics.Response;

public class TopicsWithTotalCountAdminResponseModel
{
    public List<TopicAdminResponseModelWithCommentCount> Topics { get; set; }
    public int TotalCount { get; set; }
}
