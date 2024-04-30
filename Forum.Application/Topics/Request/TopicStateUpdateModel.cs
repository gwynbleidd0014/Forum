// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Enums;

namespace Forum.Application.Topics.Request;

public class TopicStateUpdateModel
{
    public int Id { get; set; }
    public TopicState? State { get; set; }
}
