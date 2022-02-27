using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uBeac.Web;

namespace API;

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to unit roles management.
/// </summary>
/// <typeparam name="TKey">Type of unit role entity key -- TKey must have inherited from IEquatable</typeparam>
/// <typeparam name="TUnitRole">Type of unit role entity -- TUnitRole must have inherited from UnitRole</typeparam>
[Authorize(Roles = "ADMIN")]
public abstract class UnitRolesControllerBase<TKey, TUnitRole> : BaseController
    where TKey : IEquatable<TKey>
    where TUnitRole : UnitRole<TKey>
{
    protected readonly IUnitRoleService<TKey, TUnitRole> UnitRoleService;

    protected UnitRolesControllerBase(IUnitRoleService<TKey, TUnitRole> unitRoleService)
    {
        UnitRoleService = unitRoleService;
    }

    /// <summary>
    /// Creates a new unit role
    /// </summary>
    /// <returns>Returns id of created unit role</returns>
    [HttpPost]
    public virtual async Task<IApiResult<TKey>> Create([FromBody] TUnitRole unitRole, CancellationToken cancellationToken = default)
    {
        try
        {
            await UnitRoleService.Create(unitRole, cancellationToken);
            return unitRole.Id.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<TKey>();
        }
    }

    /// <summary>
    /// Updates a unit role
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
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

    /// <summary>
    /// Deletes a unit role
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
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

    /// <summary>
    /// Get all unit roles
    /// </summary>
    /// <returns>Returns all unit roles</returns>
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

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to unit roles management.
/// </summary>
/// <typeparam name="TUnitRole">Type of unit role entity -- TUnitRole must have inherited from UnitRole</typeparam>
public abstract class UnitRolesControllerBase<TUnitRole> : UnitRolesControllerBase<Guid, TUnitRole>
    where TUnitRole : UnitRole
{
    protected UnitRolesControllerBase(IUnitRoleService<TUnitRole> unitRoleService) : base(unitRoleService)
    {
    }
}