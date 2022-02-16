using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using uBeac.Web;

namespace API.Tests;

public static class Extensions
{
    public static async Task<IApiResult<TResult>> GetApiResult<TResult>(this HttpResponseMessage response)
    {
        return JsonConvert.DeserializeObject<ApiResult<TResult>>(await response.Content.ReadAsStringAsync());
    }
}