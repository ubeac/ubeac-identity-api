using System;
using System.Collections.Generic;
using FluentValidation;

namespace API;

public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(e => e.UserName)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.Password)
            .NotNull()
            .NotEmpty();
    }
}

public class LoginResponse<TUserKey>
{
    public virtual TUserKey UserId { get; set; }
    public virtual List<string> Roles { get; set; }
    public virtual string Token { get; set; }
    public virtual string RefreshToken { get; set; }
    public virtual DateTime Expiry { get; set; }
}

public class LoginResponse : LoginResponse<Guid>
{
}