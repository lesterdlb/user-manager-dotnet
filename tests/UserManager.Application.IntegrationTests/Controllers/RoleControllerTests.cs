using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Shouldly;

using UserManager.Application.Common.DTOs.Role;
using UserManager.Application.Features.Roles.Commands.CreateRole;
using UserManager.Application.Features.Roles.Commands.UpdateRole;
using UserManager.Application.IntegrationTests.Authentication;
using UserManager.Application.IntegrationTests.Base;

namespace UserManager.Application.IntegrationTests.Controllers;

[Collection("IntegrationTests")]
public class RoleControllerTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;
    private readonly Func<Task> _reseedDatabase;

    public RoleControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.HttpClient;
        _resetDatabase = factory.ResetDatabase;
        _reseedDatabase = factory.ReSeedDatabase;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        _client.DefaultRequestHeaders.Add(TestAuthHandler.UserId, Guid.NewGuid().ToString());
    }

    [Fact]
    public async Task GetAllRoles_ReturnsSuccessResult()
    {
        await _reseedDatabase();

        var response = await _client.GetAsync("/api/roles");

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<List<RoleDto>>(responseString);

        result.ShouldBeOfType<List<RoleDto>>();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetRoleById_ReturnsSuccessResult()
    {
        await _reseedDatabase();

        var response = await _client.GetAsync($"/api/roles/{Utilities.GetUserRoleId}");

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<RoleDto>(responseString);

        result.ShouldBeOfType<RoleDto>();
        result.Id.ShouldBe(Utilities.GetUserRoleId);
    }

    [Fact]
    public async Task GetRoleById_ReturnsNotFoundResult()
    {
        var response = await _client.GetAsync($"/api/roles/{Guid.NewGuid()}");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateRole_ReturnsSuccessResult()
    {
        const string roleName = "Test Role";
        var response = await _client.PostAsJsonAsync(
            "/api/roles",
            new CreateRoleCommand { Name = roleName });

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<Guid>(responseString);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBe(null);
        result.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task CreateRole_ReturnsBadRequestResult()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/roles",
            new CreateRoleCommand { Name = string.Empty });

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateRole_RoleAlreadyExists_ReturnsBadRequestResult()
    {
        await _reseedDatabase();

        var expectedErrors = new Dictionary<string, string[]>
        {
            { "role.duplicate.name", new[] { "A role with the same name already exists." } }
        };

        var response = await _client.PostAsJsonAsync(
            "/api/roles",
            new CreateRoleCommand { Name = "Admin" });

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseObj = JsonConvert.DeserializeObject<JObject>(responseBody);
        var errorsDict = responseObj?["errors"]?.ToObject<Dictionary<string, string[]>>();

        errorsDict.ShouldNotBeNull();

        Assert.Equal(expectedErrors, errorsDict);
    }

    [Fact]
    public async Task UpdateRole_ReturnsSuccessResult()
    {
        await _reseedDatabase();

        var response = await _client.PutAsJsonAsync(
            "/api/Roles",
            new UpdateRoleCommand { Id = Guid.Parse(Utilities.GetUserRoleId), Name = "Updated Role" });

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateRole_ReturnsBadRequestResult()
    {
        await _reseedDatabase();

        var response = await _client.PutAsJsonAsync(
            "/api/Roles",
            new UpdateRoleCommand { Id = Guid.Parse(Utilities.GetUserRoleId), Name = string.Empty });

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRole_ReturnsNotFoundResult()
    {
        var response = await _client.PutAsJsonAsync(
            "/api/Roles",
            new UpdateRoleCommand { Id = Guid.NewGuid(), Name = "Updated Role" });

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteRole_ReturnsSuccessResult()
    {
        await _reseedDatabase();

        var response = await _client.DeleteAsync($"/api/roles/{Utilities.GetUserRoleId}");

        response.EnsureSuccessStatusCode();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}