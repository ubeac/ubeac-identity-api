using System;
using System.Collections.Generic;
using FluentValidation;

namespace API;

public class AssignRoleRequest<TKey> where TKey : IEquatable<TKey>
{
    public virtual TKey Id { get; set; }
    public virtual List<string> Roles { get; set; }
}

public class AssignRoleRequest : AssignRoleRequest<Guid>
{
}

public class AssignRoleRequestValidator<TKey> : AbstractValidator<AssignRoleRequest<TKey>>
    where TKey : IEquatable<TKey>
{
    public AssignRoleRequestValidator()
    {
        RuleFor(e => e.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.Roles)
            .NotNull();
    }
}

public class AssignRoleRequestValidator : AbstractValidator<AssignRoleRequest>
{
    public AssignRoleRequestValidator()
    {
        RuleFor(e => e.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.Roles)
            .NotNull();
    }
}