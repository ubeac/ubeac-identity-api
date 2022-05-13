using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using Bogus;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests;

public class UsersTests : BaseTestClass, IClassFixture<Factory>
{
    private static Guid _userId;
    private static string _password;

    private readonly Factory _factory;

    public UsersTests(Factory factory)
    {
        _factory = factory;
    }

    [Fact, TestPriority(1)]
    public async Task Create_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var password = "1qaz!QAZ";
        var content = new StringContent(JsonConvert.SerializeObject(new CreateUserRequest
        {
            UserName = new Faker().Person.UserName,
            Password = password,
            FirstName = new Faker().Person.FirstName,
            LastName = new Faker().Person.LastName,
            PhoneNumber = new Faker().Person.Phone,
            PhoneNumberConfirmed = false,
            Email = new Faker().Person.Email,
            EmailConfirmed = false
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.USERS_CREATE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<Guid>();

        // Assert
        Assert.NotEqual(default, result.Data);

        // Set Static Values
        _userId = result.Data;
        _password = password;
    }

    [Fact, TestPriority(2)]
    public async Task GetAll_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();

        // Act
        var response = await client.GetAsync(Endpoints.USERS_GET_ALL);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<IEnumerable<UserResponse>>();

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
        var response = await client.GetAsync($"{Endpoints.USERS_GET_BY_ID}?id={_userId}");
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<UserResponse>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.Equal(result.Data.Id, _userId);
    }

    [Fact, TestPriority(4)]
    public async Task Update_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var content = new StringContent(JsonConvert.SerializeObject(new UpdateUserRequest
        {
            Id = _userId,
            FirstName = new Faker().Person.FirstName,
            LastName = new Faker().Person.LastName,
            PhoneNumber = new Faker().Person.Phone,
            PhoneNumberConfirmed = true,
            Email = new Faker().Person.Email,
            EmailConfirmed = true,
            LockoutEnabled = true,
            LockoutEnd = null
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.USERS_UPDATE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<bool>();

        // Assert
        Assert.True(result.Data);
    }

    [Fact, TestPriority(5)]
    public async Task AssignRoles_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var content = new StringContent(JsonConvert.SerializeObject(new AssignRoleRequest
        {
            Id = _userId,
            Roles = new List<string> { "ADMIN" }
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.USERS_ASSIGN_ROLES, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<bool>();

        // Assert
        Assert.True(result.Data);
    }

    [Fact, TestPriority(6)]
    public async Task ChangePassword_ReturnsSuccessResult()
    {
        // Arrange
        var client = await _factory.CreateAdminClient();
        var newPassword = "1QAZ!qaz";
        var content = new StringContent(JsonConvert.SerializeObject(new ChangeUserPasswordRequest
        {
            UserId = _userId,
            NewPassword = newPassword
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.USERS_CHANGE_PASSWORD, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<bool>();

        // Assert
        Assert.True(result.Data);

        // Set Static Values
        _password = newPassword;
    }
}