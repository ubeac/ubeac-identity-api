using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using uBeac.Identity;

namespace IntegrationTests;

public class Factory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }

    // Creates new HttpClient with authorization header
    public HttpClient CreateClient(string accessToken)
    {
        var client = CreateClient();
        client.SetAccessToken(accessToken);
        return client;
    }

    // Creates new HttpClient and Adds admin token to request headers
    public async Task<HttpClient> CreateAdminClient()
    {
        var client = CreateClient();
        var accessToken = await GetAdminAccessToken(client);
        client.SetAccessToken(accessToken);
        return client;
    }

    // Sends a request to login endpoint with admin username, password and Returns access token
    private async Task<string> GetAdminAccessToken(HttpClient client)
    {
        var content = new StringContent(JsonConvert.SerializeObject(new LoginRequest
        {
            UserName = SuperAdmin.USERNAME,
            Password = SuperAdmin.PASSWORD
        }), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(Endpoints.ACCOUNTS_LOGIN, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<TokenResult>();
        return result.Data.Token;
    }
}