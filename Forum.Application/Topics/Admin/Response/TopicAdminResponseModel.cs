// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Comments.Response;

namespace Forum.Application.Topics.Response;

public class TopicAdminResponseModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Text { get; set; }
    public string AuthorId { get; set; }
    public string Author { get; set; }
    public string AuthorImgPath { get; set; }
    public int DaysOld { get; set; }
    public string Status { get; set; }
    public string State { get; set; }
    public List<CommentResponseModel> Comments { get; set; }
}
