using Microsoft.AspNetCore.Identity;

namespace API;

// Inherits from UserRoleService<TUser>
public class CustomUserRoleService : UserRoleService<CustomUser>
{
    public CustomUserRoleService(UserManager<CustomUser> userManager) : base(userManager)
    {
    }

    // You can Override base methods:
    // public override async Task<bool> AddRoles(Guid userId, IEnumerable<string> roleNames, CancellationToken cancellationToken = new CancellationToken())
    // {
    //     return await base.AddRoles(userId, roleNames, cancellationToken);
    // }
}