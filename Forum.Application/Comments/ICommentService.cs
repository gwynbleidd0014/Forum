// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Comments.Request;

namespace Forum.Application.Comments;

public interface ICommentService
{
    Task AddCommentAsync(CommentRequestModel model, string userId, CancellationToken token);
    Task RemoveCommentAsync(int id, string userId, CancellationToken token);
}
