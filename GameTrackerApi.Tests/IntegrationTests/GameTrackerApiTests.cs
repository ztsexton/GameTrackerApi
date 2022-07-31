using System;
using System.Collections.Generic;
using System.Net;
using Xunit;
using System.Threading.Tasks;
using GameTrackerApi.GameAlerts;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;

namespace GameTrackerApi.Tests.IntegrationTests;

public class GameTrackerApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GameTrackerApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void CanCreateWebApplicationWithGameTrackerStartup()
    {
        var application = new WebApplicationFactory<Program>();

        var client = application.CreateClient();

        client.ShouldNotBeNull();
    }

    [Theory]
    [InlineData("/GameAlerts")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);

        if (response.Content.Headers.ContentType is null) throw new ArgumentException(nameof(response));
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType.ToString().ShouldBe("application/json; charset=utf-8");
    }

    [Theory]
    [InlineData("/health")]
    public async Task Get_HealthCheckEndpointReturnsSuccessAndCorrectContentType(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);

        if (response.Content.Headers.ContentType is null) throw new ArgumentException(nameof(response));
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType.ToString().ShouldBe("text/plain; charset=utf-8");
    }
    
    [Theory]
    [InlineData("/swagger")]
    public async Task Get_SwaggerEndpointReturnsSuccessAndCorrectContentType(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);

        if (response.Content.Headers.ContentType is null) throw new ArgumentException(nameof(response));
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType.ToString().ShouldBe("text/html; charset=utf-8");
    }

    [Fact]
    public async Task Get_GameAlertsReturnsListOfGameAlerts()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/GameAlerts");

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        var gameAlerts = JsonConvert.DeserializeObject<List<GameAlert>>(json);

        gameAlerts.ShouldBeOfType<List<GameAlert>>();
    }

    [Fact]
    public async Task Get_HealthCheckReturnsValidHealthResult()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health");
        
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Get_HomeGameAlertWithTeamThatExistsReturns200()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/gamealerts/Washington%20Nationals");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Get_HomeGameAlertWithTeamThatDoesNotExistReturns404()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/gamealerts/nonexistantteam");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
