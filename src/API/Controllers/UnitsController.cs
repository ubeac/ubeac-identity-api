using API;

// This controller has action methods (endpoints) to units management
// By inheriting from UnitsControllerBase, default action methods (endpoints) are added to this controller (like CRUD)
public class UnitsController : UnitsControllerBase<AppUnit> // You can replace AppUnit with your custom entity or Apply changes in AppUnit.cs -- Also you can use Unit (default entity) instead of AppUnit (custom entity)
{
    public UnitsController(IUnitService<AppUnit> unitService) : base(unitService)
    {
    }
}