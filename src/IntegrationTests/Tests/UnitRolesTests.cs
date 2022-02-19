using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using Newtonsoft.Json;
using uBeac.Web;
using Xunit;

namespace IntegrationTests;

public class UnitRolesTests : BaseTestClass, IClassFixture<Factory>
{
    private const string CreateUri = "/API/UnitTypes/Create";
    private const string UpdateUri = "/API/UnitTypes/Update";
    private const string DeleteUri = "/API/UnitTypes/Delete";
    private const string GetAllUri = "/API/UnitTypes/GetAll";

    private static Guid _unitRoleId;

    private readonly Factory _factory;

    public UnitRolesTests(Factory factory)
    {
        _factory = factory;
    }

    [Fact, TestPriority(1)]
    public async Task Create_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new AppUnitRole
        {
            UserName = "amir",
            Role = "admin"
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(CreateUri, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<Guid>();

        // Assert
        Assert.NotEqual(default, result.Data);

        // Set Static Values
        _unitRoleId = result.Data;
    }

    [Fact, TestPriority(2)]
    public async Task GetAll_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(GetAllUri);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<IEnumerable<AppUnitRole>>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Any());
    }

    [Fact, TestPriority(3)]
    public async Task Update_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new AppUnitRole
        {
            Id = _unitRoleId,
            UserName = "amir",
            Role = "admin"
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(UpdateUri, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<bool>();

        // Assert
        Assert.True(result.Data);
    }

    [Fact, TestPriority(4)]
    public async Task Delete_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new IdRequest
        {
            Id = _unitRoleId
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(DeleteUri, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<bool>();

        // Assert
        Assert.True(result.Data);
    }
}