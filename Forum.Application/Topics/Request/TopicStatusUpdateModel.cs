// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Enums;

namespace Forum.Application.Topics.Request;

public class TopicStatusUpdateModel
{
    public int Id { get; set; }
    public TopicStatus? Status { get; set; }
}
