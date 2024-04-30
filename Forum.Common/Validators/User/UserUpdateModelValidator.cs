// Copyright (C) TBC Bank. All Rights Reserved.

using FluentValidation;
using Forum.Application.Users.Request;

namespace Forum.Common.Validators.User;

public class UsernameChangeValidator : AbstractValidator<UserUpdateModel>
{
    public UsernameChangeValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.UserName))
            .MinimumLength(5)
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.Email))
            .EmailAddress()
            .MaximumLength(50);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .Matches("^(5\\d{8,})$").When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.FirstName)
                .MaximumLength(50);
        RuleFor(x => x.LastName)
                .MaximumLength(50);
    }
}
