// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Application.Topics.Response;

public class TopicsWithTotalCountResponseModel
{
    public List<TopicResponseModelWithCommentCount> Topics { get; set; }
    public int TotalCount { get; set; }
}
