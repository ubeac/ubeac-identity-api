using AutoMapper;

namespace API;

// This controller has action methods (endpoints) to users management
// By inheriting from UsersControllerBase, default action methods (endpoints) are added to this controller (like CRUD)
public class UsersController : UsersControllerBase<AppUser> // You can replace AppUser with your custom entity or Apply changes in AppUser.cs -- Also you can use User (default entity) instead of AppUser (custom entity)
{
    public UsersController(IUserService<AppUser> userService, IUserRoleService<AppUser> userRoleService, IMapper mapper) : base(userService, userRoleService, mapper)
    {
    }
}