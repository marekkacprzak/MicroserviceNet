using System.Text;
using Actio.Common.Auth;
using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

namespace Actio.Services.Identity.Tests.Integration.Controllers;


public class AccountControllerTests
{
    private readonly TestServer _identityServer;
    private readonly TestServer _apiServer;
    private readonly HttpClient _identityClient;
    private readonly HttpClient _apiClient;

    public AccountControllerTests()
    { 
        _apiServer = new TestServer(WebHost.CreateDefaultBuilder()
            .UseStartup<Api.Startup>());
        _apiClient = _apiServer.CreateClient();
        _identityServer = new TestServer(WebHost.CreateDefaultBuilder()
            .UseStartup<Identity.Startup>());
        _identityClient = _identityServer.CreateClient();
    }

    [Fact]
    public async Task account_controller_register_should_return_accepted()
    {
        var payload = GetPayload(new { email = "jakis5@email.com", name = "anton", password = "12345" });
        var response = await _apiClient.PostAsync(@"users/register", payload);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task account_controller_login_should_return_json_web_token()
    { 
        var payload = GetPayload(new { email = "jakis5@email.com", name = "anton", password = "12345" });
        var response = await _identityClient.PostAsync("login", payload);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var jwt = JsonConvert.DeserializeObject<JsonWebToken>(content);

        jwt.Should().NotBeNull();
        jwt?.Token.Should().NotBeEmpty();
        jwt?.Expires.Should().BeGreaterThan(0);
    }

    protected static StringContent GetPayload(object data)
    {
        var json = JsonConvert.SerializeObject(data);

        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}