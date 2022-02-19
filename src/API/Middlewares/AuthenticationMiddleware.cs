using Microsoft.AspNetCore.Http;

namespace API;

public class AuthenticationMiddleware : AuthenticationMiddlewareBase<AppUser>
{
    public AuthenticationMiddleware(RequestDelegate next) : base(next)
    {
    }
}