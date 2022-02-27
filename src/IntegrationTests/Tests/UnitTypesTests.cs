using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using Bogus;
using Newtonsoft.Json;
using uBeac.Web;
using Xunit;

namespace IntegrationTests;

public class UnitTypesTests : BaseTestClass, IClassFixture<Factory>
{
    private static string _unitCode = new Faker().Random.String();
    private static Guid _unitTypeId;

    private readonly Factory _factory;

    public UnitTypesTests(Factory factory)
    {
        _factory = factory;
    }

    [Fact, TestPriority(1)]
    public async Task Create_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var content = new StringContent(JsonConvert.SerializeObject(new AppUnitType
        {
            Code = _unitCode,
            Name = new Faker().Lorem.Word(),
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.UNIT_TYPES_CREATE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<Guid>();

        // Assert
        Assert.NotEqual(default, result.Data);

        // Set Static Values
        _unitTypeId = result.Data;
    }

    [Fact, TestPriority(2)]
    public async Task GetAll_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();

        // Act
        var response = await client.GetAsync(Endpoints.UNIT_TYPES_GET_ALL);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<IEnumerable<AppUnitType>>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Any());
    }

    [Fact, TestPriority(3)]
    public async Task Update_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var content = new StringContent(JsonConvert.SerializeObject(new AppUnitType
        {
            Id = _unitTypeId,
            Code = _unitCode,
            Name = new Faker().Lorem.Word(),
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.UNIT_TYPES_UPDATE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<bool>();

        // Assert
        Assert.True(result.Data);
    }

    [Fact, TestPriority(4)]
    public async Task Delete_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var content = new StringContent(JsonConvert.SerializeObject(new IdRequest
        {
            Id = _unitTypeId
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.UNIT_TYPES_DELETE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<bool>();

        // Assert
        Assert.True(result.Data);
    }
}