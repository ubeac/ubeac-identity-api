using System;
using System.Collections.Generic;
using FluentValidation;

namespace API;

public class RegisterRequest
{ 
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual string UserName { get; set; }
    public virtual string Email { get; set; }
    public virtual string Password { get; set; }
}

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(e => e.FirstName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(e => e.LastName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(e => e.UserName)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();

        RuleFor(e => e.Password)
            .NotNull()
            .NotEmpty();
    }
}

public class RegisterResponse<TUserKey> where TUserKey : IEquatable<TUserKey>
{
    public virtual TUserKey UserId { get; set; }
    public virtual List<string> Roles { get; set; }
    public virtual string Token { get; set; }
    public virtual string RefreshToken { get; set; }
    public virtual DateTime Expiry { get; set; }
}

public class RegisterResponse : RegisterResponse<Guid>
{
}