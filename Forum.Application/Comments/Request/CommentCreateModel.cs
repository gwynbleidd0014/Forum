// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Application.Comments.Request;

public class CommentCreateModel
{
    public string Text { get; set; }
    public string TopicId { get; set; }
    public string UserId { get; set; }
}
