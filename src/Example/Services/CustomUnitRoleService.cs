namespace Example;

// Inherits from UnitRoleService<TUnitRole>
public class CustomUnitRoleService : UnitRoleService<CustomUnitRole>
{
    public CustomUnitRoleService(IUnitRoleRepository<CustomUnitRole> repository, IApplicationContext appContext) : base(repository, appContext)
    {
    }

    // You can Override base methods:
    // public override async Task Insert(CustomUnitRole entity, CancellationToken cancellationToken = new CancellationToken())
    // {
    //     await base.Insert(entity, cancellationToken);
    // }
}