using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using Bogus;
using Newtonsoft.Json;
using uBeac;
using uBeac.Web;
using Xunit;

namespace IntegrationTests;

public class RolesTests : BaseTestClass, IClassFixture<Factory>
{
    private readonly Factory _factory;

    private static string _roleName = new Faker().Lorem.Word();
    private static Guid _roleId;

    public RolesTests(Factory factory)
    {
        _factory = factory;
    }

    [Fact, TestPriority(1)]
    public async Task Create_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var content = new StringContent(JsonConvert.SerializeObject(new AppRole
        {
            Name = _roleName
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.ROLES_CREATE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<Guid>();

        // Assert
        Assert.NotEqual(default, result.Data);

        // Set Static Values
        _roleId = result.Data;
    }

    [Fact, TestPriority(2)]
    public async Task GetAll_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();

        // Act
        var response = await client.GetAsync(Endpoints.ROLES_GET_ALL);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<IEnumerable<AppRole>>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Any());
    }

    [Fact, TestPriority(3)]
    public async Task GetById_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();

        // Act
        var response = await client.GetAsync($"{Endpoints.ROLES_GET_BY_ID}?id={_roleId}");
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<AppRole>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data.Name);
    }

    [Fact, TestPriority(4)]
    public async Task Update_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var content = new StringContent(JsonConvert.SerializeObject(new AppRole
        {
            Id = _roleId,
            Name = _roleName
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.ROLES_UPDATE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<bool>();

        // Assert
        Assert.True(result.Data);
    }

    [Fact, TestPriority(5)]
    public async Task Delete_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var content = new StringContent(JsonConvert.SerializeObject(new IdRequest
        {
            Id = _roleId,
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.ROLES_DELETE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<bool>();

        // Assert
        Assert.True(result.Data);
    }
}