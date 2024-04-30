// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Comments;
using Forum.Domain.Enums;
using Forum.Domain.Interfaces;
using Forum.Domain.Users;

namespace Forum.Domain.Topics;

public class Topic : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Text { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<Comment> Comments { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public TopicState State { get; set; }
    public TopicStatus Status { get; set; }
}
