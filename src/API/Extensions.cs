using System.Linq;
using Microsoft.AspNetCore.Http;

namespace API;

public static class Extensions
{
    public static string GetAuthToken(this HttpContext context)
        => context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
}