namespace API;

// Inherits from UnitTypeService<TUnitType>
public class CustomUnitTypeService : UnitTypeService<CustomUnitType>
{
    public CustomUnitTypeService(IUnitTypeRepository<CustomUnitType> unitTypeRepository, IApplicationContext appContext) : base(unitTypeRepository, appContext)
    {
    }

    // You can Override base methods:
    // public override async Task Insert(CustomUnitType entity, CancellationToken cancellationToken = new CancellationToken())
    // {
    //     await base.Insert(entity, cancellationToken);
    // }
}