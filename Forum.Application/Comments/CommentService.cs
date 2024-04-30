// Copyright (C) TBC Bank. All Rights Reserved.

using Forum.Application.Comments.Request;
using Forum.Application.Errors.CustomErrors;
using Forum.Application.Resourses;
using Forum.Application.Topics.Admin;
using Forum.Domain.Comments;
using Forum.Domain.Enums;
using Mapster;

namespace Forum.Application.Comments;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IAdminTopicService _topicService;

    public CommentService(ICommentRepository commentRepository, IAdminTopicService topicService)
    {
        _commentRepository = commentRepository;
        _topicService = topicService;
    }

    public async Task AddCommentAsync(CommentRequestModel model, string userId, CancellationToken token)
    {
        var comment = model.Adapt<CommentCreateModel>();
        var topic = await _topicService.GetTopicByIdWithoutCommentsAsync(int.Parse(comment.TopicId), token);
        if (topic.Status == TopicStatus.Inactive.ToString())
            throw new Forbiden(ErrorMessages.CommentArchiveDenied);
        if (topic.State != TopicState.Show.ToString())
            throw new NotFound(ErrorMessages.TopicNotFound);

        comment.UserId = userId;

        await _commentRepository.AddCommentAsync(comment.Adapt<Comment>(), token);
    }
    public async Task RemoveCommentAsync(int id, string userId, CancellationToken token)
    {
        var comment = await _commentRepository.GetAsync(id, token);

        if (comment == null || comment.UserId.ToString() != userId)
            throw new NotFound(ErrorMessages.CommentNotFound);

        await _commentRepository.RemoveCommentAsync(id, token);
    }

}
