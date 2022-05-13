using System;
using FluentValidation;

namespace API;

// Change user password by admin
public class ChangeUserPasswordRequest<TKey> where TKey : IEquatable<TKey>
{
    public virtual TKey UserId { get; set; }
    public virtual string NewPassword { get; set; }
}

public class ChangeUserPasswordRequest : ChangeUserPasswordRequest<Guid>
{
}

public class ChangeUserPasswordRequestValidator<TKey> : AbstractValidator<ChangeUserPasswordRequest<TKey>>
    where TKey : IEquatable<TKey>
{
    public ChangeUserPasswordRequestValidator()
    {
        RuleFor(e => e.UserId)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.NewPassword)
            .NotNull()
            .NotEmpty();
    }
}

public class ChangeUserPasswordRequestValidator : AbstractValidator<ChangeUserPasswordRequest>
{
    public ChangeUserPasswordRequestValidator()
    {
        RuleFor(e => e.UserId)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.NewPassword)
            .NotNull()
            .NotEmpty();
    }
}

// Change password of authenticated user
public class ChangeAccountPasswordRequest
{
    public virtual string CurrentPassword { get; set; }
    public virtual string NewPassword { get; set; }
}

public class ChangeAccountPasswordRequestValidator : AbstractValidator<ChangeAccountPasswordRequest>
{
    public ChangeAccountPasswordRequestValidator()
    {
        RuleFor(e => e.CurrentPassword)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.NewPassword)
            .NotNull()
            .NotEmpty();
    }
}