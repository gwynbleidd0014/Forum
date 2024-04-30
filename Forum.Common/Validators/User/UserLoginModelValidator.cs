// Copyright (C) TBC Bank. All Rights Reserved.

using FluentValidation;
using Forum.Application.Users.Request;

namespace Forum.Common.Validators.User;
public class UserLoginModelValidator : AbstractValidator<UserLoginModel>
{
    public UserLoginModelValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(5);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}
