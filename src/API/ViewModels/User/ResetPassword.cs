using FluentValidation;

namespace API;

public class ResetPasswordRequest
{
    public virtual string UserName { get; set; }
    public virtual string Token { get; set; }
    public virtual string NewPassword { get; set; }
}

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(e => e.UserName)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.Token)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.NewPassword)
            .NotNull()
            .NotEmpty();
    }
}