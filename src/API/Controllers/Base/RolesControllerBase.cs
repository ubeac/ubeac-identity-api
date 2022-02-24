using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using uBeac.Web;

namespace API;

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to roles management.
/// </summary>
/// <typeparam name="TRoleKey">Type of role entity key -- TKey must have inherited from IEquatable</typeparam>
/// <typeparam name="TRole">Type of role entity -- TRole must have inherited from Role</typeparam>
public abstract class RolesControllerBase<TRoleKey, TRole> : BaseController
   where TRoleKey : IEquatable<TRoleKey>
   where TRole : Role<TRoleKey>
{
    protected readonly IRoleService<TRoleKey, TRole> RoleService;

    protected RolesControllerBase(IRoleService<TRoleKey, TRole> roleService)
    {
        RoleService = roleService;
    }

    /// <summary>
    /// Creates a new role
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IApiResult<TRoleKey>> Create([FromBody] TRole role, CancellationToken cancellationToken = default)
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

    /// <summary>
    /// Updates a role
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
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

    /// <summary>
    /// Deletes a role
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
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

    /// <summary>
    /// Get all roles
    /// </summary>
    /// <returns>Returns all roles</returns>
    [HttpGet]
    public virtual async Task<IApiListResult<TRole>> GetAll(CancellationToken cancellationToken = default)
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

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to roles management.
/// </summary>
/// <typeparam name="TRole">Type of role entity -- TRole must have inherited from Role</typeparam>
public abstract class RolesControllerBase<TRole> : RolesControllerBase<Guid, TRole>
   where TRole : Role
{
    protected RolesControllerBase(IRoleService<TRole> roleService) : base(roleService)
    {
    }
}