using FluentValidation;

namespace API;

public class ForgotPasswordRequest
{
    public virtual string UserName { get; set; }
}

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(e => e.UserName)
            .NotNull()
            .NotEmpty();
    }
}