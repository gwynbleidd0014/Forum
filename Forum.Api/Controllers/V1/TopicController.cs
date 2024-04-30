// Copyright (C) TBC Bank. All Rights Reserved.

using System.Security.Claims;
using Asp.Versioning;
using Forum.Application.Topics;
using Forum.Application.Topics.Request;
using Forum.Application.Topics.Response;
using Forum.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.V1;

[ApiVersion("1.0")]
public class TopicController : CustomBaseController
{
    private readonly ITopicService _topicService;
    private readonly IUserService _userService;

    public TopicController(ITopicService topicService, IUserService userService)
    {
        _topicService = topicService;
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<TopicResponseModel> GetTopicByIdAsync([FromRoute] int id, CancellationToken token)
    {
        return await _topicService.GetTopicByIdAsync(id, token);
    }

    [HttpGet]
    public async Task<TopicsWithTotalCountResponseModel> GetAllAsync(CancellationToken token, [FromQuery] int pg = 1, [FromQuery] int pageSize = 10)
    {
        var recSkip = (pg - 1) * pageSize;
        var result = await _topicService.GetAllAsync(recSkip, pageSize, token);

        return result;
    }

    [HttpGet("archive")]
    public async Task<TopicsWithTotalCountResponseModel> GetArchivedAsync(CancellationToken token, [FromQuery] int pg = 1, [FromQuery] int pageSize = 10)
    {
        var recSkip = (pg - 1) * pageSize;
        var result = await _topicService.GetArchivedAsync(recSkip, pageSize, token);

        return result;
    }

    [HttpGet("email")]
    public async Task<TopicsWithTotalCountResponseModel> GetAllByEmailAsync([FromQuery] string email, CancellationToken token, [FromQuery] int pg = 1, [FromQuery] int pageSize = 10)
    {
        var user = await _userService.FindByEmailAsync(email, token);

        var recSkip = (pg - 1) * pageSize;
        var result = await _topicService.GetUserTopicsWithCommentCountAsync(user.Id, recSkip, pageSize, token);

        return result;
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task CreateTopicAsync(TopicRequestModel model, CancellationToken token)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        await _topicService.CreateTopicAsync(model, userId, token);
    }
}
