using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using uBeac.Web;
using Xunit;

namespace API.Tests;

public class UnitRolesTests : BaseTestClass, IClassFixture<Factory>
{
    private const string InsertUri = "/API/UnitTypes/Insert";
    private const string UpdateUri = "/API/UnitTypes/Update";
    private const string DeleteUri = "/API/UnitTypes/Delete";
    private const string AllUri = "/API/UnitTypes/All";

    private static Guid _unitRoleId;

    private readonly Factory _factory;

    public UnitRolesTests(Factory factory)
    {
        _factory = factory;
    }

    [Fact, TestPriority(1)]
    public async Task Insert_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new AppUnitRole
        {
            UserName = "amir",
            Role = "admin"
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(InsertUri, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<Guid>();

        // Assert
        Assert.NotEqual(default, result.Data);

        // Set Static Values
        _unitRoleId = result.Data;
    }

    [Fact, TestPriority(2)]
    public async Task All_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(AllUri);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<IEnumerable<AppUnitRole>>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Any());
    }

    [Fact, TestPriority(3)]
    public async Task Replace_ReturnsSuccessApiResult()
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