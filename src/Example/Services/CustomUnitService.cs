namespace Example;

// Inherits from UnitService<TUnit>
public class CustomUnitService : UnitService<CustomUnit>
{
    public CustomUnitService(IUnitRepository<CustomUnit> repository, IApplicationContext appContext) : base(repository, appContext)
    {
    }

    // You can Override base methods:
    // public override async Task Insert(CustomUnit entity, CancellationToken cancellationToken = new CancellationToken())
    // {
    //     await base.Insert(entity, cancellationToken);
    // }
}