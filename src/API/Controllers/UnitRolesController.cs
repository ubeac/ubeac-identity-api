namespace API;

public class UnitRolesController : UnitRolesControllerBase<AppUnitRole>
{
    public UnitRolesController(IUnitRoleService<AppUnitRole> unitRoleService) : base(unitRoleService)
    {
    }
}