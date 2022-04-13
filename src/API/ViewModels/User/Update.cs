using System;
using FluentValidation;

namespace API;

// Update user info by admin
public class UpdateUserRequest<TKey> where TKey : IEquatable<TKey>
{ 
    public virtual TKey Id { get; set; }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual string PhoneNumber { get; set; }
    public virtual bool PhoneNumberConfirmed { get; set; }
    public virtual string Email { get; set; }
    public virtual bool EmailConfirmed { get; set; }
    public virtual bool LockoutEnabled { get; set; }
    public virtual DateTimeOffset? LockoutEnd { get; set; }
    public virtual bool Enabled { get; set; }
}

public class UpdateUserRequest : UpdateUserRequest<Guid>
{
}

public class UpdateUserRequestValidator<TKey> : AbstractValidator<UpdateUserRequest<TKey>>
    where TKey : IEquatable<TKey>
{
    public UpdateUserRequestValidator()
    {
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
    }
}

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
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
    }
}

// Update info of authenticated user
public class UpdateAccountRequest
{
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual string PhoneNumber { get; set; }
    public virtual string Email { get; set; }
}

public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequest>
{
    public UpdateAccountRequestValidator()
    {
        RuleFor(e => e.FirstName)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.LastName)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.PhoneNumber)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();
    }
}