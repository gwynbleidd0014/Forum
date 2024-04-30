// Copyright (C) TBC Bank. All Rights Reserved.

using FluentValidation;
using Forum.Application.Comments.Request;

namespace Forum.Common.Validators.Comment;

public class CommentRequestModelValidator : AbstractValidator<CommentRequestModel>
{
    public CommentRequestModelValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.TopicId)
            .NotEmpty();
    }
}
