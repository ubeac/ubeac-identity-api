using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using uBeac.Web;

namespace API;

public class UnitRolesController : UnitRolesControllerBase<CustomUnitRole>
{
    public UnitRolesController(IUnitRoleService<CustomUnitRole> unitRoleService) : base(unitRoleService)
    {
    }
}

public abstract class UnitRolesControllerBase<TKey, TUnitRole> : BaseController
    where TKey : IEquatable<TKey>
    where TUnitRole : UnitRole<TKey>
{
    protected readonly IUnitRoleService<TKey, TUnitRole> UnitRoleService;

    protected UnitRolesControllerBase(IUnitRoleService<TKey, TUnitRole> unitRoleService)
    {
        UnitRoleService = unitRoleService;
    }

    [HttpPost]
    public virtual async Task<IApiResult<bool>> Create([FromBody] TUnitRole unitRole, CancellationToken cancellationToken = default)
    {
        try
        {
            await UnitRoleService.Insert(unitRole, cancellationToken);
            return true.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<bool>();
        }
    }

    [HttpPost]
    public virtual async Task<IApiResult<bool>> Update([FromBody] TUnitRole unitRole, CancellationToken cancellationToken = default)
    {
        try
        {
            await UnitRoleService.Update(unitRole, cancellationToken);
            return true.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<bool>();
        }
    }

    [HttpPost]
    public virtual async Task<IApiResult<bool>> Delete([FromBody] IdRequest<TKey> request, CancellationToken cancellationToken = default)
    {
        try
        {
            await UnitRoleService.Delete(request.Id, cancellationToken);
            return true.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<bool>();
        }
    }

    [HttpGet]
    public virtual async Task<IApiListResult<TUnitRole>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            var unitRoles = await UnitRoleService.GetAll(cancellationToken);
            return new ApiListResult<TUnitRole>(unitRoles);
        }
        catch (Exception ex)
        {
            return ex.ToApiListResult<TUnitRole>();
        }
    }
}

public abstract class UnitRolesControllerBase<TUnitRole> : UnitRolesControllerBase<Guid, TUnitRole>
    where TUnitRole : UnitRole
{
    protected UnitRolesControllerBase(IUnitRoleService<TUnitRole> unitRoleService) : base(unitRoleService)
    {
    }
}