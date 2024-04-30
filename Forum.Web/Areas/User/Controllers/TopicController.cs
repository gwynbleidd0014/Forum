// Copyright (C) TBC Bank. All Rights Reserved.

using System.Security.Claims;
using Forum.Application.Topics;
using Forum.Application.Topics.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Areas.User.Controllers;

[Area("User")]
[Authorize(Roles = "User")]
public class TopicController : Controller
{
    private readonly ITopicService _topicService;

    public TopicController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromForm] TopicRequestModel model, CancellationToken token)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        await _topicService.CreateTopicAsync(model, userId, token);

        return RedirectToAction("Index", "Topic", new { area = ""});
    }
}
