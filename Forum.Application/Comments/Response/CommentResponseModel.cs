// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Application.Comments.Response;

public class CommentResponseModel
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string UserId { get; set; }
    public string Author { get; set; }
    public string AuthorImgPath { get; set; }
    public int DaysOld { get; set; }
    public DateTime CreatedAt { get; set; }
}
