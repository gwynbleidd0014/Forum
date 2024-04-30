// Copyright (C) TBC Bank. All Rights Reserved.

using FluentValidation;
using Forum.Application.Topics.Request;

namespace Forum.Application.Validators.Topics;

public class TopicRequestModelValidator : AbstractValidator<TopicRequestModel>
{
    public TopicRequestModelValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(1000);
    }
}
