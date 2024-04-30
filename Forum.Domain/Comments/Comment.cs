// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Interfaces;
using Forum.Domain.Topics;
using Forum.Domain.Users;

namespace Forum.Domain.Comments;

public class Comment : IEntity, ISoftDelete
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public int Id { get; set; }
    public string Text { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int TopicId { get; set; }
    public Topic Topic { get; set; }
    public bool IsDeleted { get; set; }
}
