using System.Reflection;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using uBeac.Enums;

namespace API;

public class EnumsController : BaseController
{
    [HttpGet]
    public IListResult<EnumModel> GetAll(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var enums = Assembly.GetEntryAssembly().GetReferencedAssemblies().GetEnums();
        return enums.ToListResult();
    }
}