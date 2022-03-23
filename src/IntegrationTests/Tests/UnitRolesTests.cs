using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using Newtonsoft.Json;
using uBeac;
using uBeac.Web;
using Xunit;

namespace IntegrationTests;

public class UnitRolesTests : BaseTestClass, IClassFixture<Factory>
{
    private static Guid _unitRoleId;

    private readonly Factory _factory;

    public UnitRolesTests(Factory factory)
    {
        _factory = factory;
    }

    [Fact, TestPriority(1)]
    public async Task Create_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var content = new StringContent(JsonConvert.SerializeObject(new AppUnitRole
        {
            UserName = "amir",
            Role = "admin"
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.UNIT_ROLES_CREATE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<Guid>();

        // Assert
        Assert.NotEqual(default, result.Data);

        // Set Static Values
        _unitRoleId = result.Data;
    }

    [Fact, TestPriority(2)]
    public async Task GetAll_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();

        // Act
        var response = await client.GetAsync(Endpoints.UNIT_ROLES_GET_ALL);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<IEnumerable<AppUnitRole>>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Any());
    }

    [Fact, TestPriority(3)]
    public async Task Update_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var content = new StringContent(JsonConvert.SerializeObject(new AppUnitRole
        {
            Id = _unitRoleId,
            UserName = "amir",
            Role = "admin"
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.UNIT_ROLES_UPDATE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<bool>();

        // Assert
        Assert.True(result.Data);
    }

    [Fact, TestPriority(4)]
    public async Task Delete_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var content = new StringContent(JsonConvert.SerializeObject(new IdRequest
        {
            Id = _unitRoleId
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.UNIT_ROLES_DELETE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<bool>();

        // Assert
        Assert.True(result.Data);
    }
}