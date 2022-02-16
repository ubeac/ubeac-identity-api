using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using uBeac.Web;

namespace API;

public class UnitTypesController : UnitTypesControllerBase<CustomUnitType>
{
    public UnitTypesController(IUnitTypeService<CustomUnitType> unitTypeService) : base(unitTypeService)
    {
    }
}

public abstract class UnitTypesControllerBase<TKey, TUnitType> : BaseController
    where TKey : IEquatable<TKey>
    where TUnitType : UnitType<TKey>
{
    protected readonly IUnitTypeService<TKey, TUnitType> UnitTypeService;

    protected UnitTypesControllerBase(IUnitTypeService<TKey, TUnitType> unitTypeService)
    {
        UnitTypeService = unitTypeService;
    }

    [HttpPost]
    public virtual async Task<IApiResult<TKey>> Create([FromBody] TUnitType unitType, CancellationToken cancellationToken = default)
    {
        try
        {
            await UnitTypeService.Insert(unitType, cancellationToken);
            return unitType.Id.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<TKey>();
        }
    }

    [HttpPost]
    public virtual async Task<IApiResult<bool>> Update([FromBody] TUnitType unitType, CancellationToken cancellationToken = default)
    {
        try
        {
            await UnitTypeService.Update(unitType, cancellationToken);
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
            await UnitTypeService.Delete(request.Id, cancellationToken);
            return true.ToApiResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiResult<bool>();
        }
    }

    [HttpGet]
    public virtual async Task<IApiListResult<TUnitType>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            var unitTypes = await UnitTypeService.GetAll(cancellationToken);
            return unitTypes.ToApiListResult();
        }
        catch (Exception ex)
        {
            return ex.ToApiListResult<TUnitType>();
        }
    }
}

public abstract class UnitTypesControllerBase<TUnitType> : UnitTypesControllerBase<Guid, TUnitType>
    where TUnitType : UnitType
{
    protected UnitTypesControllerBase(IUnitTypeService<TUnitType> unitTypeService) : base(unitTypeService)
    {
    }
}