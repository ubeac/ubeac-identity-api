using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uBeac.Web;

namespace API;

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to unit types management.
/// </summary>
/// <typeparam name="TKey">Type of unit type entity key -- TKey must have inherited from IEquatable</typeparam>
/// <typeparam name="TUnitType">Type of unit type entity -- TUnit must have inherited from UnitType</typeparam>
[Authorize(Roles = "ADMIN")]
public abstract class UnitTypesControllerBase<TKey, TUnitType> : BaseController
    where TKey : IEquatable<TKey>
    where TUnitType : UnitType<TKey>
{
    protected readonly IUnitTypeService<TKey, TUnitType> UnitTypeService;

    protected UnitTypesControllerBase(IUnitTypeService<TKey, TUnitType> unitTypeService)
    {
        UnitTypeService = unitTypeService;
    }

    /// <summary>
    /// Creates a new unit type
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IResult<TKey>> Create([FromBody] TUnitType unitType, CancellationToken cancellationToken = default)
    {
        await UnitTypeService.Create(unitType, cancellationToken);
        return unitType.Id.ToResult();
    }

    /// <summary>
    /// Updates a unit type
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IResult<bool>> Update([FromBody] TUnitType unitType, CancellationToken cancellationToken = default)
    {
        await UnitTypeService.Update(unitType, cancellationToken);
        return true.ToResult();
    }

    /// <summary>
    /// Deletes a unit type
    /// </summary>
    /// <returns>If an exception is thrown, returns false, otherwise true</returns>
    [HttpPost]
    public virtual async Task<IResult<bool>> Delete([FromBody] IdRequest<TKey> request, CancellationToken cancellationToken = default)
    {
        await UnitTypeService.Delete(request.Id, cancellationToken);
        return true.ToResult();
    }

    /// <summary>
    /// Get all unit types
    /// </summary>
    /// <returns>Returns all unit types</returns>
    [HttpGet]
    public virtual async Task<IListResult<TUnitType>> GetAll(CancellationToken cancellationToken = default)
    {
        var unitTypes = await UnitTypeService.GetAll(cancellationToken);
        return unitTypes.ToListResult();
    }
}

/// <summary>
/// This class is an abstract controller (base controller) that has base action methods (endpoints) to unit types management.
/// </summary>
/// <typeparam name="TUnitType">Type of unit type entity -- TUnit must have inherited from UnitType</typeparam>
public abstract class UnitTypesControllerBase<TUnitType> : UnitTypesControllerBase<Guid, TUnitType>
    where TUnitType : UnitType
{
    protected UnitTypesControllerBase(IUnitTypeService<TUnitType> unitTypeService) : base(unitTypeService)
    {
    }
}