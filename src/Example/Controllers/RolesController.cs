using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using uBeac.Web;

namespace Example;

[Authorize(Roles = "ADMIN")]
public class RolesController : RolesControllerBase<CustomRole>
{
    public RolesController(IRoleService<CustomRole> roleService) : base(roleService)
    {
    }
}

public abstract class RolesControllerBase<TRoleKey, TRole> : BaseController
   where TRoleKey : IEquatable<TRoleKey>
   where TRole : Role<TRoleKey>
{
    protected readonly IRoleService<TRoleKey, TRole> RoleService;

    protected RolesControllerBase(IRoleService<TRoleKey, TRole> roleService)
    {
        RoleService = roleService;
    }

    [HttpPost]
    public virtual async Task<IApiResult<TRoleKey>> Insert([FromBody] TRole role, CancellationToken cancellationToken = default)
    {
        try
        {
            await RoleService.Insert(role, cancellationToken);
            return role.Id.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<TRoleKey>();
        }
    }

    [HttpPost]
    public virtual async Task<IApiResult<bool>> Update([FromBody] TRole role, CancellationToken cancellationToken = default)
    {
        try
        {
            await RoleService.Update(role, cancellationToken);
            return true.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<bool>();
        }
    }

    [HttpPost]
    public virtual async Task<IApiResult<bool>> Delete([FromBody] IdRequest<TRoleKey> request, CancellationToken cancellationToken = default)
    {
        try
        {
            await RoleService.Delete(request.Id, cancellationToken);
            return true.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<bool>();
        }
    }

    [HttpGet]
    public virtual async Task<IApiListResult<TRole>> All(CancellationToken cancellationToken = default)
    {
        try
        {
            var roles = await RoleService.GetAll(cancellationToken);
            return roles.ToApiListResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiListResult<TRole>();
        }
    }
}

public abstract class RolesControllerBase<TRole> : RolesControllerBase<Guid, TRole>
   where TRole : Role
{
    protected RolesControllerBase(IRoleService<TRole> roleService) : base(roleService)
    {
    }
}

