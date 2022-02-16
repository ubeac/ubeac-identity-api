using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using uBeac.Web;

namespace API;

public class UnitsController : UnitsControllerBase<CustomUnit>
{
    public UnitsController(IUnitService<CustomUnit> unitService) : base(unitService)
    {
    }
}

public abstract class UnitsController<TKey, TUnit> : BaseController
    where TKey : IEquatable<TKey>
    where TUnit : Unit<TKey>
{
    protected readonly IUnitService<TKey, TUnit> UnitService;

    protected UnitsController(IUnitService<TKey, TUnit> unitService)
    {
        UnitService = unitService;
    }

    [HttpPost]
    public virtual async Task<IApiResult<TKey>> Insert([FromBody] TUnit unit, CancellationToken cancellationToken = default)
    {
        try
        {
            await UnitService.Insert(unit, cancellationToken);
            return unit.Id.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<TKey>();
        }
    }

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

    [HttpGet]
    public virtual async Task<IApiListResult<TUnit>> All(CancellationToken cancellationToken = default)
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
}

public abstract class UnitsControllerBase<TUnit> : UnitsController<Guid, TUnit>
    where TUnit : Unit
{
    protected UnitsControllerBase(IUnitService<TUnit> unitService) : base(unitService)
    {
    }
}