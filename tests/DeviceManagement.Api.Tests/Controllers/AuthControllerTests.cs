using System.Net;
using System.Net.Http.Json;
using Application.Dtos;
using FluentAssertions;

namespace DeviceManagement.Api.Tests.Controllers;

[TestClass]
public class AuthControllerTests
{
    private static TestWebApplicationFactory _factory = null!;
    private static HttpClient _client = null!;


    [ClassInitialize]
    public static void ClassSetup(TestContext context)
    {
        _factory = new TestWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [TestMethod]
    public async Task Login_ValidCredentials_Returns200WithToken()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login",
            new LoginRequest("alice@example.com", "Password123!"));

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<AuthResponse>();
        body.Should().NotBeNull();
        body!.Token.Should().NotBeNullOrWhiteSpace();
        body.Email.Should().Be("alice@example.com");
    }

    [TestMethod]
    public async Task Login_UnknownEmail_Returns401()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login",
            new LoginRequest("nobody@example.com", "Password123!"));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [TestMethod]
    public async Task Login_WrongPassword_Returns401()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login",
            new LoginRequest("alice@example.com", "WrongPassword!"));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [TestMethod]
    public async Task Register_NewEmail_Returns201WithToken()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest(
                "newuser@example.com", "Password123!",
                "New User", "Developer", "Amsterdam"));

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadFromJsonAsync<AuthResponse>();
        body!.Token.Should().NotBeNullOrWhiteSpace();
    }

    [TestMethod]
    public async Task Register_DuplicateEmail_Returns409Conflict()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest(
                "alice@example.com", "Password123!",
                "Alice Again", "Dev", "London"));

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
