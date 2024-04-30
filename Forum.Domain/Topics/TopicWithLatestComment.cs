// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Comments;

namespace Forum.Domain.Topics;

public class TopicWithLatestComment
{
    public int TopicId { get; set; }
    public DateTime ModifiedAt { get; set; }
    public Comment? LatestComment { get; set; }
}
