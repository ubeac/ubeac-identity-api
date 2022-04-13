using System;
using FluentValidation;

namespace API;

public class UpdateRoleRequest<TKey> where TKey : IEquatable<TKey>
{
    public virtual TKey Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
}

public class UpdateRoleRequest : UpdateRoleRequest<Guid>
{
}

public class UpdateRoleRequestValidator<TKey> : AbstractValidator<UpdateRoleRequest<TKey>>
    where TKey : IEquatable<TKey>
{
    public UpdateRoleRequestValidator()
    {
        RuleFor(e => e.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(e => e.Description)
            .MaximumLength(4096);
    }
}

public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
{
    public UpdateRoleRequestValidator()
    {
        RuleFor(e => e.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(e => e.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(e => e.Description)
            .MaximumLength(4096);
    }
}