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
    public virtual async Task<IResult<TKey>> Create([FromBody] TUnit unit, CancellationToken cancellationToken = default)
    {
        await UnitService.Create(unit, cancellationToken);
        return unit.Id.ToResult();
    }

    /// <summary>
    /// Updates a role
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IResult<bool>> Update([FromBody] TUnit unit, CancellationToken cancellationToken = default)
    {
        await UnitService.Update(unit, cancellationToken);
        return true.ToResult();
    }

    /// <summary>
    /// Deletes a role
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IResult<bool>> Delete([FromBody] IdRequest<TKey> request, CancellationToken cancellationToken = default)
    {
        await UnitService.Delete(request.Id, cancellationToken);
        return true.ToResult();
    }

    /// <summary>
    /// Get all units
    /// </summary>
    /// <returns>Returns all units</returns>
    [HttpGet]
    public virtual async Task<IListResult<TUnit>> GetAll(CancellationToken cancellationToken = default)
    {
        var units = await UnitService.GetAll(cancellationToken);
        return units.ToListResult();
    }

    /// <summary>
    /// Get unit info by id
    /// </summary>
    /// <returns>Returns unit info</returns>
    [HttpGet]
    public virtual async Task<IResult<TUnit>> GetById([FromQuery] IdRequest<TKey> request, CancellationToken cancellationToken = default)
    {
        var unit = await UnitService.GetById(request.Id, cancellationToken);
        return unit.ToResult();
    }

    /// <summary>
    /// Get units by parent id
    /// </summary>
    /// <returns>Returns units</returns>
    [HttpGet]
    public virtual async Task<IListResult<TUnit>> GetByParentId([FromQuery] IdRequest<TKey> request, CancellationToken cancellationToken = default)
    {
        var units = await UnitService.GetByParentId(request.Id, cancellationToken);
        return units.ToListResult();
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