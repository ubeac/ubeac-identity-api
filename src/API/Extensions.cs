using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API;

public static class Extensions
{
    public static string GetAuthToken(this HttpContext context)
        => context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

    public static IServiceCollection DisableAutomaticModelStateValidation(this IServiceCollection services)
        => services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
}