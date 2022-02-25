using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests;

public class UsersTests : BaseTestClass, IClassFixture<Factory>
{
    private const string CreateUri = "/API/Users/Create";
    private const string UpdateUri = "/API/Users/Update";
    private const string AssignRolesUri = "/API/Users/AssignRoles";
    private const string ChangePasswordUri = "/API/Users/ChangePassword";
    private const string GetByIdUri = "/API/Users/GetById";
    private const string GetAllUri = "/API/Users/GetAll";

    private static Guid _userId;
    private static string _password;

    private readonly Factory _factory;

    public UsersTests(Factory factory)
    {
        _factory = factory;
    }

    [Fact, TestPriority(1)]
    public async Task Create_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var password = "1qaz!QAZ";
        var content = new StringContent(JsonConvert.SerializeObject(new InsertUserRequest
        {
            UserName = "john",
            Password = password,
            PhoneNumber = "+98",
            PhoneNumberConfirmed = false,
            Email = "john@doe.com",
            EmailConfirmed = false
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(CreateUri, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<Guid>();

        // Assert
        Assert.NotEqual(default, result.Data);

        // Set Static Values
        _userId = result.Data;
        _password = password;
    }

    [Fact, TestPriority(2)]
    public async Task GetAll_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(GetAllUri);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<IEnumerable<UserResponse>>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Any());
    }

    [Fact, TestPriority(3)]
    public async Task GetById_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"{GetByIdUri}?id={_userId}");
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<UserResponse>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.Equal(result.Data.Id, _userId);
    }

    [Fact, TestPriority(4)]
    public async Task Update_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new UpdateUserRequest
        {
            Id = _userId,
            PhoneNumber = "+9810",
            PhoneNumberConfirmed = true,
            Email = "john@doe.com",
            EmailConfirmed = true,
            LockoutEnabled = true,
            LockoutEnd = null
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(UpdateUri, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<bool>();

        // Assert
        Assert.True(result.Data);
    }

    [Fact, TestPriority(5)]
    public async Task AssignRoles_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new AssignRoleRequest
        {
            Id = _userId,
            Roles = new List<string> { "admin" }
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(AssignRolesUri, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<bool>();

        // Assert
        Assert.True(result.Data);
    }

    [Fact, TestPriority(6)]
    public async Task ChangePassword_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newPassword = "1QAZ!qaz";
        var content = new StringContent(JsonConvert.SerializeObject(new ChangeUserPasswordRequest
        {
            UserId = _userId,
            CurrentPassword = _password,
            NewPassword = newPassword
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(ChangePasswordUri, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<bool>();

        // Assert
        Assert.True(result.Data);

        // Set Static Values
        _password = newPassword;
    }
}