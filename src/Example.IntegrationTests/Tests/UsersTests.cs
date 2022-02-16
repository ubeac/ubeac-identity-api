using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace API.Tests;

public class UsersTests : BaseTestClass, IClassFixture<Factory>
{
    private const string InsertUri = "/API/Users/Insert";
    private const string UpdateUri = "/API/Users/Update";
    private const string ChangePasswordUri = "/API/Users/ChangePassword";
    private const string FindUri = "/API/Users/Find";
    private const string AllUri = "/API/Users/All";

    private static Guid _userId;
    private static string _password;

    private readonly Factory _factory;

    public UsersTests(Factory factory)
    {
        _factory = factory;
    }

    [Fact, TestPriority(1)]
    public async Task Insert_ReturnsSuccessApiResult()
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
        var response = await client.PostAsync(InsertUri, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<Guid>();

        // Assert
        Assert.NotEqual(default, result.Data);

        // Set Static Values
        _userId = result.Data;
        _password = password;
    }

    [Fact, TestPriority(2)]
    public async Task All_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(AllUri);
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<IEnumerable<UserViewModel>>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Any());
    }

    [Fact, TestPriority(3)]
    public async Task Find_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"{FindUri}?id={_userId}");
        response.EnsureSuccessStatusCode();
        var result = await response.GetApiResult<UserViewModel>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.Equal(result.Data.Id, _userId);
    }

    [Fact, TestPriority(4)]
    public async Task Replace_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new ReplaceUserRequest
        {
            Id = _userId,
            PhoneNumber = "+98",
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
    public async Task ChangePassword_ReturnsSuccessApiResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newPassword = "1QAZ!qaz";
        var content = new StringContent(JsonConvert.SerializeObject(new ChangePasswordRequest
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