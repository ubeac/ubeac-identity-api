using FluentValidation;

namespace API;

public class CreateRoleRequest
{
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
}

public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(e => e.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(e => e.Description)
            .MaximumLength(4096);
    }
}