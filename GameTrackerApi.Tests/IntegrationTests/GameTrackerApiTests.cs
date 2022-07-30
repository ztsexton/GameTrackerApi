using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
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
    public async Task HelloWorldTests()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                
            });
        
        var client = application.CreateClient();
    }

    [Theory]
    [InlineData("/weatherforecast")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();
        
        response.Content.Headers.ContentType.ToString().ShouldBe("application/json; charset=utf-8");
    }
}