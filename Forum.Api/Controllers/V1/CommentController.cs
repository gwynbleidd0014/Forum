// Copyright (C) TBC Bank. All Rights Reserved.

using System.Security.Claims;
using Forum.Application.Comments;
using Forum.Application.Comments.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.V1;

[Authorize(Roles = "User")]
public class CommentController : CustomBaseController
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public async Task Add(CommentRequestModel model, CancellationToken token)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _commentService.AddCommentAsync(model, userId, token);
    }
    [HttpDelete("{id}")]
    public async Task Remove(int id, CancellationToken token)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _commentService.RemoveCommentAsync(id, userId, token);
    }

}
