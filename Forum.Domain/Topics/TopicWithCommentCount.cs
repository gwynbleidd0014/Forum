// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Domain.Topics;

public class TopicWithCommentCount
{
    public Topic Topic { get; set; }
    public int CommentCount { get; set; }
}
