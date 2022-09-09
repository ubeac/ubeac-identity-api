using System.Collections.Generic;
using FluentValidation;

namespace API;

public class CreateUserRequest
{
    public virtual string UserName { get; set; }
    public virtual string Password { get; set; }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual string PhoneNumber { get; set; }
    public virtual bool PhoneNumberConfirmed { get; set; }
    public virtual string Email { get; set; }
    public virtual bool EmailConfirmed { get; set; }
    public virtual List<string> Roles { get; set; }
    public virtual bool Enabled { get; set; } = true;
}

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(e => e.UserName)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.Password)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.FirstName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(e => e.LastName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(e => e.PhoneNumber)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();

        RuleFor(e => e.Roles)
            .NotNull();
    }
}