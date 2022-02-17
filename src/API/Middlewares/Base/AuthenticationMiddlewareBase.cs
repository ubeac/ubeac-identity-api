using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace API;

public class AuthenticationMiddlewareBase<TKey, TUser>
    where TKey : IEquatable<TKey>
    where TUser : User<TKey>
{
    private readonly RequestDelegate _next;
    private readonly IUserService<TKey, TUser> _userService;
    private readonly IUserRoleService<TKey, TUser> _userRoleService;

    public AuthenticationMiddlewareBase(RequestDelegate next, IUserService<TKey, TUser> userService, IUserRoleService<TKey, TUser> userRoleService)
    {
        _next = next;
        _userService = userService;
        _userRoleService = userRoleService;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            var user = await GetUserAsync(context.Request);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.NormalizedUserName)
                };

                var userRoles = await _userRoleService.GetRolesForUser(user.Id);
                var userRoleClaims = userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole));
                claims.AddRange(userRoleClaims);

                var identity = new ClaimsIdentity(claims);
                context.User = new ClaimsPrincipal(identity);
            }
        }
        catch (Exception)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
        finally
        {
            await _next(context);
        }
    }

    private async Task<TUser> GetUserAsync(HttpRequest request)
    {
        try
        {
            if (request.Headers.TryGetValue("Authorization", out StringValues authHeader) && authHeader.ToString().StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                var accessToken = authHeader.ToString().Substring("Bearer".Length).Trim();
                var principal = GetPrincipal(accessToken);
                var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
                return await _userService.GetById((TKey)(object)userId);
            }
        }
        catch (Exception)
        {
            return null;
        }

        return null;
    }

    private ClaimsPrincipal GetPrincipal(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(accessToken);
        if (jwtToken == null) return null;
        var identity = new ClaimsIdentity(jwtToken.Claims);
        return new ClaimsPrincipal(identity);
    }
}

public class AuthenticationMiddlewareBase<TUser> : AuthenticationMiddlewareBase<Guid, TUser> where TUser : User
{
    public AuthenticationMiddlewareBase(RequestDelegate next, IUserService<TUser> userService, IUserRoleService<TUser> userRoleService) : base(next, userService, userRoleService)
    {
    }
}