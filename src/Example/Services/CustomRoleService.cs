using Microsoft.AspNetCore.Identity;

namespace Example;

// Inherits from RoleService<TRole>
public class CustomRoleService : RoleService<CustomRole>
{
    public CustomRoleService(RoleManager<CustomRole> roleManager) : base(roleManager)
    {
    }

    // You can Override base methods:
    // public override async Task Insert(CustomRole role, CancellationToken cancellationToken = new CancellationToken())
    // {
    //     await base.Insert(role, cancellationToken);
    // }
}