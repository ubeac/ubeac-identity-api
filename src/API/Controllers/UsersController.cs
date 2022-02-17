namespace API;

public class UsersController : UsersControllerBase<AppUser>
{
    public UsersController(IUserService<AppUser> userService) : base(userService)
    {
    }
}