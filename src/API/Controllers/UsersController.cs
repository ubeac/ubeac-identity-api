using AutoMapper;

namespace API;

public class UsersController : UsersControllerBase<AppUser>
{
    public UsersController(IUserService<AppUser> userService, IMapper mapper) : base(userService, mapper)
    {
    }
}