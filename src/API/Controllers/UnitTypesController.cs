namespace API;

// This controller has action methods (endpoints) to unit types management
// By inheriting from UnitTypesControllerBase, default action methods (endpoints) are added to this controller (like CRUD)
public class UnitTypesController : UnitTypesControllerBase<AppUnitType> // You can replace AppUnitType with your custom entity or Apply changes in AppUnitType.cs -- Also you can use UnitType (default entity) instead of AppUnitType (custom entity)
{
    public UnitTypesController(IUnitTypeService<AppUnitType> unitTypeService) : base(unitTypeService)
    {
    }
}