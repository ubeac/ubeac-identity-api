using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Example;

public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService<CustomUser>>();
        var userRoleService = context.HttpContext.RequestServices.GetRequiredService<IUserRoleService<CustomUser>>();

        if (string.IsNullOrWhiteSpace(Roles)) return;

        var roles = Roles.Trim(' ').Split(',');
        if (!roles.Any()) return;

        var userId = await userService.GetCurrentUserId();
        var userRoles = await userRoleService.GetRolesForUser(userId);
        var authorized = roles.Any(role => userRoles.Contains(role));

        if (!authorized) context.Result = new ForbidResult();
    }
}