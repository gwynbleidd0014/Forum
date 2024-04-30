// Copyright (C) TBC Bank. All Rights Reserved.

using FluentValidation;
using Forum.Application.Users.Request;

namespace Forum.Common.Validators.User;

public class UserRegisterModelValidator : AbstractValidator<UserRegisterModel>
{
    public UserRegisterModelValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(5);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MinimumLength(5);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.Password);
    }
}
