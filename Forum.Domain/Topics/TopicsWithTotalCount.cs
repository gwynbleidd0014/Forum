// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Domain.Topics;

public class TopicsWithTotalCount
{
    public List<TopicWithCommentCount> Topics { get; set; }
    public int TotalCount { get; set; }
}
