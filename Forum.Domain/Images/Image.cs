// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Interfaces;
using Forum.Domain.Users;

namespace Forum.Domain.Images;

public class Image : IEntity
{
    public int Id { get; set; }
    public string Path { get; set; }
    public string AbsolutePath { get; set; }
    public User User { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
