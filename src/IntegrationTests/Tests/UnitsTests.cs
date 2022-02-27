using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using Bogus;
using Newtonsoft.Json;
using uBeac.Identity;
using uBeac.Web;
using Xunit;

namespace IntegrationTests;

public class UnitsTests : BaseTestClass, IClassFixture<Factory>
{
    private const string CreateUri = "/API/Units/Create";
    private const string UpdateUri = "/API/Units/Update";
    private const string DeleteUri = "/API/Units/Delete";
    private const string GetAllUri = "/API/Units/GetAll";

    private static string _unitCode = new Faker().Random.String();
    private static string _unitType = new Faker().Random.String();

    private static Guid _unitId;

    private readonly Factory _factory;

    public UnitsTests(Factory factory)
    {
        _factory = factory;
    }

    [Fact, TestPriority(1)]
    public async Task Create_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new AppUnit
        {
            Name = new Faker().Lorem.Word(),
            Code = _unitCode,
            Type = _unitType
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(CreateUri, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<Guid>();

        // Assert
        Assert.NotEqual(default, result.Data);

        // Set Static Values
        _unitId = result.Data;
    }

    [Fact, TestPriority(2)]
    public async Task GetAll_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(GetAllUri);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<IEnumerable<AppUnit>>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Any());
    }

    [Fact, TestPriority(3)]
    public async Task Update_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new Unit
        {
            Id = _unitId,
            Name = new Faker().Lorem.Word(),
            Code = _unitCode,
            Type = _unitType
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
            Id = _unitId
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(DeleteUri, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<bool>();

        // Assert
        Assert.True(result.Data);
    }
}