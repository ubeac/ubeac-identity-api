using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using Bogus;
using Newtonsoft.Json;
using uBeac.Identity;
using Xunit;

namespace IntegrationTests;

public class AccountsTests : BaseTestClass, IClassFixture<Factory>
{
    private readonly Factory _factory;

    private static string _userName = new Faker().Person.UserName;
    private static string _email = new Faker().Person.Email;
    private static string _password = "1qaz!QAZ";
    private static Guid _userId;
    private static string _accessToken;
    private static string _refreshToken;

    public AccountsTests(Factory factory)
    {
        _factory = factory;
    }

    [Fact, TestPriority(1)]
    public async Task Register_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new RegisterRequest
        {
            UserName = _userName,
            FirstName = new Faker().Person.FirstName,
            LastName = new Faker().Person.LastName,
            Email = _email,
            Password = _password
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.ACCOUNTS_REGISTER, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<RegisterResponse>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.NotEqual(default, result.Data.UserId);
        Assert.NotEmpty(result.Data.Token);
        Assert.NotEmpty(result.Data.RefreshToken);

        // Set Static Values
        _userId = result.Data.UserId;
        _accessToken = result.Data.Token;
        _refreshToken = result.Data.RefreshToken;
    }

    [Fact, TestPriority(2)]
    public async Task Logout_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient(_accessToken);

        // Act
        var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, Endpoints.ACCOUNTS_LOGOUT));
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<bool>();

        // Assert
        Assert.True(result.Data);
    }

    [Fact, TestPriority(3)]
    public async Task Login_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new LoginRequest
        {
            UserName = _userName,
            Password = _password
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.ACCOUNTS_LOGIN, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<SignInResult>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.NotEqual(default, result.Data.UserId);
        Assert.NotEmpty(result.Data.Token);
        Assert.NotEmpty(result.Data.RefreshToken);

        // Set Static Values
        _userId = result.Data.UserId;
        _accessToken = result.Data.Token;
        _refreshToken = result.Data.RefreshToken;
    }

    [Fact, TestPriority(4)]
    public async Task RefreshToken_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new RefreshTokenRequest
        {
            Token = _accessToken,
            RefreshToken = _refreshToken
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.ACCOUNTS_REFRESH_TOKEN, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<SignInResult>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.NotEqual(default, result.Data.UserId);
        Assert.NotEmpty(result.Data.Token);
        Assert.NotEmpty(result.Data.RefreshToken);

        // Set Static Values
        _userId = result.Data.UserId;
        _accessToken = result.Data.Token;
        _refreshToken = result.Data.RefreshToken;
    }

    [Fact, TestPriority(5)]
    public async Task Update_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient(_accessToken);
        var content = new StringContent(JsonConvert.SerializeObject(new UpdateAccountRequest
        {
            FirstName = new Faker().Person.FirstName,
            LastName = new Faker().Person.LastName,
            Email = _email,
            PhoneNumber = new Faker().Person.Phone
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.ACCOUNTS_UPDATE, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<bool>();

        // Assert
        Assert.True(result.Data);
    }

    [Fact, TestPriority(6)]
    public async Task GetCurrentUser_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient(_accessToken);

        // Act
        var response = await client.GetAsync(Endpoints.ACCOUNTS_GET_CURRENT_USER);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<UserResponse>();

        // Assert
        Assert.NotNull(result.Data);
        Assert.NotEqual(default, result.Data.Id);
        Assert.NotEmpty(result.Data.UserName);
        Assert.NotEmpty(result.Data.FirstName);
        Assert.NotEmpty(result.Data.LastName);
        Assert.NotEmpty(result.Data.Email);
        Assert.NotEmpty(result.Data.PhoneNumber);
    }

    [Fact, TestPriority(6)]
    public async Task GetCurrentRoles_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient(_accessToken);

        // Act
        var response = await client.GetAsync(Endpoints.ACCOUNTS_GET_CURRENT_ROLES);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<IEnumerable<string>>();

        // Assert
        Assert.NotNull(result.Data);
    }

    [Fact, TestPriority(7)]
    public async Task ForgotPassword_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(new ForgotPasswordRequest
        {
            UserName = _userName
        }), Encoding.UTF8, "application/json");
    
        // Act
        var response = await client.PostAsync(Endpoints.ACCOUNTS_FORGOT_PASSWORD, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<bool>();
    
        // Assert
        Assert.True(result.Data);
    }

    [Fact, TestPriority(8)]
    public async Task ChangePassword_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.SetAccessToken(_accessToken);
        var newPassword = "1QAZ!qaz";
        var content = new StringContent(JsonConvert.SerializeObject(new ChangeAccountPasswordRequest
        {
            CurrentPassword = _password,
            NewPassword = newPassword
        }), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(Endpoints.ACCOUNTS_CHANGE_PASSWORD, content);
        response.EnsureSuccessStatusCode();
        var result = await response.GetResult<bool>();

        // Assert
        Assert.True(result.Data);

        // Set Static Values
        _password = newPassword;
    }
}