namespace API;

// This controller has action methods (endpoints) to unit roles management
// By inheriting from UnitRolesControllerBase, default action methods (endpoints) are added to this controller (like CRUD)
public class UnitRolesController : UnitRolesControllerBase<AppUnitRole> // You can replace AppUnitRole with your custom entity or Apply changes in AppUnitRole.cs -- Also you can use UnitRole (default entity) instead of AppUnitRole (custom entity)
{
    public UnitRolesController(IUnitRoleService<AppUnitRole> unitRoleService) : base(unitRoleService)
    {
    }
}