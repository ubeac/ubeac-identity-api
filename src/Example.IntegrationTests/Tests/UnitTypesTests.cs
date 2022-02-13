using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using uBeac.Identity;
using Xunit;

namespace Example.Tests;

public class UnitTypesTests : BaseTestClass, IClassFixture<Factory>
{
    private const string InsertUri = "/API/UnitTypes/Insert";
    private const string UpdateUri = "/API/UnitTypes/Update";
    private const string DeleteUri = "/API/UnitTypes/Delete";
    private const string AllUri = "/API/UnitTypes/All";

    private static Guid _unitTypeId;

    private readonly Factory _factory;

    public UnitTypesTests(Factory factory)
    {
        _factory = factory;
    }

    [Fact, TestPriority(1)]
    public async Task Insert_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new CustomUnitType
        {
            Code = "HQ",
            Name = "Headquarter"
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(InsertUri, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<Guid>();

        // Assert
        Assert.NotEqual(default, result.Data);

        // Set Static Values
        _unitTypeId = result.Data;
    }

    [Fact, TestPriority(2)]
    public async Task All_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(AllUri);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<IEnumerable<CustomUnitType>>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Any());
    }

    [Fact, TestPriority(3)]
    public async Task Replace_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new CustomUnitType
        {
            Id = _unitTypeId,
            Code = "HQ",
            Name = "Headquarter"
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
        var content = new StringContent(JsonConvert.SerializeObject(new Unit
        {
            Id = _unitTypeId
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(DeleteUri, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<bool>();

        // Assert
        Assert.True(result.Data);
    }
}