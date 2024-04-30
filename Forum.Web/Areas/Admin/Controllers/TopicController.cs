// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Topics.Admin;
using Forum.Application.Topics.Request;
using Forum.Common.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class TopicController : Controller
{
    private readonly IAdminTopicService _topicService;
    private readonly IConfiguration _config;

    public TopicController(IAdminTopicService topicService, IConfiguration config)
    {
        _topicService = topicService;
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> Topics(CancellationToken token, int pg = 1)
    {
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
        var pageSize = _config.GetValue<int>("Constants:PageSize");
        var recSkip = (pg - 1) * pageSize;

        var result = await _topicService.GetUserTopicsWithCommentCountAsync(id, recSkip, pageSize, token);

        var pager = new Pager(result.TotalCount, pg, pageSize);
        ViewBag.Pager = pager;
        ViewBag.UserId = id;

        return View(result.Topics);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken token)
    {
        var topics = await _topicService.GetTopicByIdAsync(id, token);
        return View(topics);
    }

    [HttpPost]
    public async Task<IActionResult> EditState(TopicStateUpdateModel model, CancellationToken token)
    {
        await _topicService.UpdateStateAsync(model, token);

        return RedirectToAction(nameof(Edit), new { id = model.Id});
    }

    [HttpPost]
    public async Task<IActionResult> EditStatus(TopicStatusUpdateModel model, CancellationToken token)
    {
        await _topicService.UpdateStatusAsync(model, token);

        return RedirectToAction(nameof(Edit), new { id = model.Id });
    }
}
