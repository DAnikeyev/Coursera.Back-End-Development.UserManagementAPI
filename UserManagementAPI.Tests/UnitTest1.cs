using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using UserManagementAPI.Contracts;

namespace UserManagementAPI.Tests;

public sealed class UsersApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string ValidToken = "techhive-internal-token";
    private readonly WebApplicationFactory<Program> _factory;

    public UsersApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetUsers_WithoutToken_ReturnsUnauthorized()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/users");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Contains("Unauthorized", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task GetUserById_WhenUserDoesNotExist_ReturnsNotFound()
    {
        using var client = CreateAuthenticatedClient();

        var response = await client.GetAsync($"/api/users/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateUser_WithInvalidEmail_ReturnsBadRequest()
    {
        using var client = CreateAuthenticatedClient();

        var response = await client.PostAsJsonAsync("/api/users", new
        {
            firstName = "Nora",
            lastName = "Stone",
            email = "not-an-email",
            department = "HR",
            isActive = true
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("Validation failed", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task CreateUser_WithValidPayload_ReturnsCreatedUser()
    {
        using var client = CreateAuthenticatedClient();

        var response = await client.PostAsJsonAsync("/api/users", new
        {
            firstName = "Iris",
            lastName = "Cole",
            email = "iris.cole@techhive.local",
            department = "Security",
            isActive = true
        });

        response.EnsureSuccessStatusCode();

        var createdUser = await response.Content.ReadFromJsonAsync<UserResponse>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdUser);
        Assert.Equal("Iris", createdUser!.FirstName);
        Assert.Equal("Security", createdUser.Department);
    }

    private HttpClient CreateAuthenticatedClient()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ValidToken);
        return client;
    }
}
