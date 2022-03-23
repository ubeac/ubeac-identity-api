using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using uBeac;

namespace IntegrationTests;

public static class Extensions
{
    public static async Task<IResult<TResult>> GetResult<TResult>(this HttpResponseMessage response)
    {
        return JsonConvert.DeserializeObject<Result<TResult>>(await response.Content.ReadAsStringAsync());
    }

    public static void SetAccessToken(this HttpClient client, string accessToken)
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
    }
}