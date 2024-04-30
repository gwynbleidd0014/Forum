// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Domain.Comments;

namespace Forum.Application.Comments;

public interface ICommentRepository
{
    Task AddCommentAsync(Comment model, CancellationToken token);
    Task RemoveCommentAsync(int id, CancellationToken token);
    Task<Comment?> GetAsync(int id, CancellationToken token);
}
