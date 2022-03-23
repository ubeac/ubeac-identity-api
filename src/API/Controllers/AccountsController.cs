using AutoMapper;

namespace API;

// This controller has action methods (endpoints) to accounts management
// By inheriting from AccountsControllerBase, default action methods (endpoints) are added to this controller (like Register, Login, etc.)
public class AccountsController : AccountsControllerBase<AppUser> // You can replace AppUser with your custom entity or Apply changes in AppUser.cs -- Also you can use User (default entity) instead of AppUser (custom entity)
{
    public AccountsController(IUserService<AppUser> userService, IUserRoleService<AppUser> userRoleService, IMapper mapper) : base(userService, userRoleService, mapper)
    {
    }
}