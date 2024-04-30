// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Application.Topics.Request;

public class TopicCreateModel
{
    public string? Name { get; set; }
    public string? Text { get; set; }
    public int UserId { get; set; }
}
