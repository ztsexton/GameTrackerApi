using System;
using System.Collections.Generic;
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
    public async Task CanCreateWebApplicationWithGameTrackerStartup()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => { });

        var client = application.CreateClient();
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
}