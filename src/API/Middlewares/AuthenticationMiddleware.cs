using Microsoft.AspNetCore.Http;

namespace API;

public class AuthenticationMiddleware : AuthenticationMiddlewareBase<AppUser>
{
    public AuthenticationMiddleware(RequestDelegate next, IUserService<AppUser> userService, IUserRoleService<AppUser> userRoleService) : base(next, userService, userRoleService)
    {
    }
}