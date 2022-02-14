using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Example;

// Inherits from UserService<TUser>
public class CustomUserService : UserService<CustomUser>
{
    public CustomUserService(UserManager<CustomUser> userManager, IJwtTokenProvider jwtTokenProvider, IHttpContextAccessor httpContextAccessor, JwtOptions jwtOptions, IUserTokenRepository userTokenRepository) : base(userManager, jwtTokenProvider, httpContextAccessor, jwtOptions, userTokenRepository)
    {
    }

    // You can Override base methods:
    // public override async Task Insert(CustomUser user, string password, CancellationToken cancellationToken = new CancellationToken())
    // {
    //     await base.Insert(user, password, cancellationToken);
    // }
}