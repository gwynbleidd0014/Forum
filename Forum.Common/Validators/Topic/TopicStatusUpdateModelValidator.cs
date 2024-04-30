// Copyright (C) TBC Bank. All Rights Reserved.

using FluentValidation;
using Forum.Application.Topics.Request;
using Forum.Domain.Enums;

namespace Forum.Common.Validators.Topic;

public class TopicStatusUpdateModelValidator : AbstractValidator<TopicStatusUpdateModel>
{
    public TopicStatusUpdateModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Status)
            .Must(x => x == TopicStatus.Active || x == TopicStatus.Inactive);
    }
}
