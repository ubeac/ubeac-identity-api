using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uBeac.Web;

namespace API;

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to units management.
/// </summary>
/// <typeparam name="TKey">Type of unit entity key -- TKey must have inherited from IEquatable</typeparam>
/// <typeparam name="TUnit">Type of unit entity -- TUnit must have inherited from Unit</typeparam>
[Authorize(Roles = "ADMIN")]
public abstract class UnitsController<TKey, TUnit> : BaseController
    where TKey : IEquatable<TKey>
    where TUnit : Unit<TKey>
{
    protected readonly IUnitService<TKey, TUnit> UnitService;

    protected UnitsController(IUnitService<TKey, TUnit> unitService)
    {
        UnitService = unitService;
    }

    /// <summary>
    /// Creates a new unit
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IApiResult<TKey>> Create([FromBody] TUnit unit, CancellationToken cancellationToken = default)
    {
        try
        {
            await UnitService.Create(unit, cancellationToken);
            return unit.Id.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<TKey>();
        }
    }

    /// <summary>
    /// Updates a role
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IApiResult<bool>> Update([FromBody] TUnit unit, CancellationToken cancellationToken = default)
    {
        try
        {
            await UnitService.Update(unit, cancellationToken);
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
    public virtual async Task<IApiResult<bool>> Delete([FromBody] IdRequest<TKey> request, CancellationToken cancellationToken = default)
    {
        try
        {
            await UnitService.Delete(request.Id, cancellationToken);
            return true.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<bool>();
        }
    }

    /// <summary>
    /// Get all units
    /// </summary>
    /// <returns>Returns all units</returns>
    [HttpGet]
    public virtual async Task<IApiListResult<TUnit>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            var units = await UnitService.GetAll(cancellationToken);
            return units.ToApiListResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiListResult<TUnit>();
        }
    }

    /// <summary>
    /// Get unit info by id
    /// </summary>
    /// <returns>Returns unit info</returns>
    [HttpGet]
    public virtual async Task<IApiResult<TUnit>> GetById([FromQuery] IdRequest<TKey> request, CancellationToken cancellationToken = default)
    {
        try
        {
            var unit = await UnitService.GetById(request.Id, cancellationToken);
            return unit.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<TUnit>();
        }
    }

    /// <summary>
    /// Get units by parent id
    /// </summary>
    /// <returns>Returns units</returns>
    [HttpGet]
    public virtual async Task<IApiListResult<TUnit>> GetByParentId([FromQuery] IdRequest<TKey> request, CancellationToken cancellationToken = default)
    {
        try
        {
            var units = await UnitService.GetByParentId(request.Id, cancellationToken);
            return units.ToApiListResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiListResult<TUnit>();
        }
    }
}

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to units management.
/// </summary>
/// <typeparam name="TUnit">Type of unit entity -- TUnit must have inherited from Unit</typeparam>
public abstract class UnitsControllerBase<TUnit> : UnitsController<Guid, TUnit>
    where TUnit : Unit
{
    protected UnitsControllerBase(IUnitService<TUnit> unitService) : base(unitService)
    {
    }
}