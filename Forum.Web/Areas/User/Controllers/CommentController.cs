// Copyright (C) TBC Bank. All Rights Reserved.

using System.Security.Claims;
using Forum.Application.Comments;
using Forum.Application.Comments.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Areas.User.Controllers;

[Area("User")]
[Authorize(Roles = "User")]
public class CommentController : Controller
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public IActionResult Add(string topicId, CancellationToken token)
    {
        ViewBag.TopicId = topicId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(CommentRequestModel model, CancellationToken token)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _commentService.AddCommentAsync(model, userId, token);

        return RedirectToAction("Get", "Topic", new { area = "", id = model.TopicId});

    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, string topicId, CancellationToken token)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _commentService.RemoveCommentAsync(id, userId, token);

        return RedirectToAction("Get", "Topic", new { id = topicId, area = ""});
    }

}
