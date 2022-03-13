using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace API;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService<AppUser> userService, IUserRoleService<AppUser> userRoleService, IJwtTokenProvider tokenProvider)
    {
        try
        {
            var user = await GetUserAsync(context.Request, userService);
            if (user != null)
            {
                var token = context.GetAuthToken();
                var tokenValidationResult = tokenProvider.ValidateToken(token);
                if (tokenValidationResult is false) throw new Exception();

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.NormalizedUserName)
                };
                if (!string.IsNullOrWhiteSpace(user.NormalizedEmail)) claims.Add(new Claim(ClaimTypes.Email, user.NormalizedEmail));
                if (!string.IsNullOrWhiteSpace(user.PhoneNumber)) claims.Add(new Claim(ClaimTypes.Email, user.PhoneNumber));

                var userRoles = await userRoleService.GetRolesForUser(user.Id);
                var userRoleClaims = userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole));
                claims.AddRange(userRoleClaims);

                var identity = new ClaimsIdentity(claims, "DynamicAuth");
                context.User = new ClaimsPrincipal(identity);
            }
        }
        catch (Exception)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        finally
        {
            await _next(context);
        }
    }

    private async Task<AppUser> GetUserAsync(HttpRequest request, IUserService<AppUser> userService)
    {
        try
        {
            if (request.Headers.TryGetValue("Authorization", out StringValues authHeader) && authHeader.ToString().StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                var accessToken = authHeader.ToString().Substring("Bearer".Length).Trim();
                var principal = GetPrincipal(accessToken);
                var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
                return await userService.GetById(Guid.Parse(userId));
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