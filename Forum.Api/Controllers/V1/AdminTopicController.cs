// Copyright (C) TBC Bank. All Rights Reserved.

using Asp.Versioning;
using Forum.Application.Topics.Admin;
using Forum.Application.Topics.Request;
using Forum.Application.Topics.Response;
using Forum.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.V1;

[Authorize(Roles = "Admin")]
[ApiVersion("1.0")]
public class AdminTopicController : CustomBaseController
{
    private readonly IAdminTopicService _topicService;
    private readonly IUserService _userService;

    public AdminTopicController(IAdminTopicService topicService, IUserService userService)
    {
        _topicService = topicService;
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<TopicAdminResponseModel> GetTopicByIdAsync([FromRoute] int id, CancellationToken token)
    {
        return await _topicService.GetTopicByIdAsync(id, token);
    }

    [HttpGet]
    public async Task<TopicsWithTotalCountAdminResponseModel> GetAllAsync(CancellationToken token, [FromQuery] int pg = 1, [FromQuery] int pageSize = 10)
    {
        var recSkip = (pg - 1) * pageSize;
        var result = await _topicService.GetAllAsync(recSkip, pageSize, token);

        return result;
    }

    [HttpGet("archive")]
    public async Task<TopicsWithTotalCountAdminResponseModel> GetArchivedAsync(CancellationToken token, [FromQuery] int pg = 1, [FromQuery] int pageSize = 10)
    {
        var recSkip = (pg - 1) * pageSize;
        var result = await _topicService.GetArchivedAsync(recSkip, pageSize, token);

        return result;
    }

    [HttpGet("email")]
    public async Task<TopicsWithTotalCountAdminResponseModel> GetAllByEmail([FromQuery] string email, CancellationToken token, [FromQuery] int pg = 1, [FromQuery] int pageSize = 10)
    {
        var user = await _userService.FindByEmailAsync(email, token);

        var recSkip = (pg - 1) * pageSize;
        var result = await _topicService.GetUserTopicsWithCommentCountAsync(user.Id, recSkip, pageSize, token);

        return result;
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("state")]
    public async Task UpdateStateAsync(TopicStateUpdateModel model, CancellationToken token)
    {
        await _topicService.UpdateStateAsync(model, token);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("status")]
    public async Task UpdateStatusAsync(TopicStatusUpdateModel model, CancellationToken token)
    {
        await _topicService.UpdateStatusAsync(model, token);
    }
}
