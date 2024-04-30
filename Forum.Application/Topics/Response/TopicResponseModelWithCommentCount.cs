// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Application.Topics.Response;

public class TopicResponseModelWithCommentCount
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Text { get; set; }
    public string AuthorId { get; set; }
    public string Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
    public int CommentCount { get; set; }
}
