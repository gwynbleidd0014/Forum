// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Domain.Interfaces;

public interface IBanable
{
    public bool IsBanned { get; set; }
    public DateTime? BannedUntil { get; set;}
}
