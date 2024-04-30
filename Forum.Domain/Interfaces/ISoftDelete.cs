// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Domain.Interfaces;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}
