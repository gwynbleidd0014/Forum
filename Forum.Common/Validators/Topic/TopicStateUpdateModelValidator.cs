// Copyright (C) TBC Bank. All Rights Reserved.

using FluentValidation;
using Forum.Application.Topics.Request;
using Forum.Domain.Enums;

namespace Forum.Common.Validators.Topic;

public class TopicStateUpdateModelValidator : AbstractValidator<TopicStateUpdateModel>
{
    public TopicStateUpdateModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.State)
            .Must(x => x == TopicState.Show || x == TopicState.Hide);
    }
}
