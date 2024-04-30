// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Comments.Response;

namespace Forum.Application.Topics.Response;

public class TopicResponseModelWithLastestComment
{
    public int TopicId { get; set; }
    public DateTime ModifiedAt { get; set; }
    public CommentResponseModel? LatestComment { get; set; }
}
