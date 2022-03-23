using AutoMapper;

namespace API;

// This controller has action methods (endpoints) to role management
// By inheriting from RolesControllerBase, default action methods (endpoints) are added to this controller (like CRUD)
public class RolesController : RolesControllerBase<AppRole>  // You can replace AppRole with your custom entity or Apply changes in AppRole.cs -- Also you can use Role (default entity) instead of AppRole (custom entity)
{
    public RolesController(IRoleService<AppRole> roleService, IMapper mapper) : base(roleService, mapper)
    {
    }
}