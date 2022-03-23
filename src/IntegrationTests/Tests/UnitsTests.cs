﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using Bogus;
using Newtonsoft.Json;
using uBeac;
using uBeac.Identity;
using uBeac.Web;
using Xunit;

namespace IntegrationTests;

public class UnitsTests : BaseTestClass, IClassFixture<Factory>
{
    private static string _unitCode = new Faker().Random.String();
    private static string _unitType = new Faker().Random.String();

    private static Guid _unitId;

    private readonly Factory _factory;

    public UnitsTests(Factory factory)
    {
        _factory = factory;
    }

    [Fact, TestPriority(1)]
    public async Task Create_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var content = new StringContent(JsonConvert.SerializeObject(new AppUnit
        {
            Name = new Faker().Lorem.Word(),
            Code = _unitCode,
            Type = _unitType
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.UNITS_CREATE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<Guid>();

        // Assert
        Assert.NotEqual(default, result.Data);

        // Set Static Values
        _unitId = result.Data;
    }

    [Fact, TestPriority(2)]
    public async Task GetAll_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();

        // Act
        var response = await client.GetAsync(Endpoints.UNITS_GET_ALL);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<IEnumerable<AppUnit>>();

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
        var response = await client.GetAsync($"{Endpoints.UNITS_GET_BY_ID}?id={_unitId}");
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<AppUnit>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.Equal(result.Data.Id, _unitId);
        Assert.NotEmpty(result.Data.Code);
        Assert.NotEmpty(result.Data.Type);
        Assert.NotEmpty(result.Data.Name);
    }

    [Fact, TestPriority(4)]
    public async Task GetByParentId_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();

        // Act
        var response = await client.GetAsync($"{Endpoints.UNITS_GET_BY_PARENT_ID}?id={_unitId}");
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<IEnumerable<AppUnit>>();

        // Assert
        Assert.NotNull(result.Data);
    }

    [Fact, TestPriority(5)]
    public async Task Update_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var content = new StringContent(JsonConvert.SerializeObject(new Unit
        {
            Id = _unitId,
            Name = new Faker().Lorem.Word(),
            Code = _unitCode,
            Type = _unitType
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.UNITS_UPDATE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<bool>();

        // Assert
        Assert.True(result.Data);
    }

    [Fact, TestPriority(6)]
    public async Task Delete_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var content = new StringContent(JsonConvert.SerializeObject(new IdRequest
        {
            Id = _unitId
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.UNITS_DELETE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<bool>();

        // Assert
        Assert.True(result.Data);
    }
}