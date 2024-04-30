// Copyright (C) TBC Bank. All Rights Reserved.

using FluentValidation;
using Forum.Application.Resourses;
using Forum.Application.Users.Request;

namespace Forum.Common.Validators.User;

public class PasswordChangeValidatior : AbstractValidator<UserPasswordChangeModel>
{
    public PasswordChangeValidatior()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty()
            .Matches(@"^(?=.*[!#])(?=.*[A-Z])(?=.*[0-9])(?=.*[a-z]).{8,}$")
            .WithMessage(ErrorMessages.PasswordMust)
            .MinimumLength(8);

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(@"^(?=.*[!#])(?=.*[A-Z])(?=.*[0-9])(?=.*[a-z]).{8,}$")
            .WithMessage(ErrorMessages.PasswordMust)
            .MinimumLength(8);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.NewPassword);
    }
}
