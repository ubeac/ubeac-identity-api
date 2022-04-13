using FluentValidation;

namespace API;

public class RefreshTokenRequest
{
    public virtual string Token { get; set; }
    public virtual string RefreshToken { get; set; }
}

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(e => e.Token)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.RefreshToken)
            .NotNull()
            .NotEmpty();
    }
}