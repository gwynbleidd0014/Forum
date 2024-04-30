// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Topics;
using Forum.Common.Paging;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers;

public class TopicController : Controller
{
    private readonly ITopicService _topicService;
    private readonly IConfiguration _config;

    public TopicController(ITopicService topicService, IConfiguration config)
    {
        _topicService = topicService;
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int id, CancellationToken token)
    {
        if (User.IsInRole("Admin"))
            RedirectToAction("Edit", "Topic", new { area = "Admin" });
        var topic = await _topicService.GetTopicByIdAsync(id, token);

        return View(topic);
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken token, int pg = 1)
    {
        if (User.IsInRole("Admin"))
            return RedirectToAction("Topics", "Topic", new { area = "Admin", pg });

        var pageSize = _config.GetValue<int>("Constants:PageSize");
        var recSkip = (pg - 1) * pageSize;

        var result = await _topicService.GetAllAsync(recSkip, pageSize, token);

        var pager = new Pager(result.TotalCount, pg, pageSize);
        ViewBag.Pager = pager;

        return View(result.Topics);
    }

    [HttpGet]
    public async Task<IActionResult> Archive(CancellationToken token, int pg = 1)
    {
        if (User.IsInRole("Admin"))
            return RedirectToAction("Archive", "Topic", new { area = "Admin", pg });

        var pageSize = _config.GetValue<int>("Constants:PageSize");
        var recSkip = (pg - 1) * pageSize;

        var result = await _topicService.GetArchivedAsync(recSkip, pageSize, token);

        var pager = new Pager(result.TotalCount, pg, pageSize);
        ViewBag.Pager = pager;

        return View(result.Topics);
    }

    [HttpGet]
    public async Task<IActionResult> UserTopics(int id, CancellationToken token, int pg = 1)
    {
        if (User.IsInRole("Admin"))
            return RedirectToAction("UserTopics", "Topic", new { area = "Admin", id, pg });

        var pageSize = _config.GetValue<int>("Constants:PageSize");
        var recSkip = (pg - 1) * pageSize;

        var result = await _topicService.GetUserTopicsWithCommentCountAsync(id, recSkip, pageSize, token);

        var pager = new Pager(result.TotalCount, pg, pageSize);
        ViewBag.Pager = pager;
        ViewBag.UserId = id;

        return View(result.Topics);
    }
}
