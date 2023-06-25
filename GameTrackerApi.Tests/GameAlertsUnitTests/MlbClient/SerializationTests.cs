using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using GameTrackerApi.GameAlerts.Mlb;
using Shouldly;
using Xunit;

namespace GameTrackerApi.Tests.GameAlertsUnitTests.MlbClient;

public class SerializationTests
{
    [Fact]
    public async Task DeserializationIsCaseInsensitive()
    {
        var json = await File.ReadAllTextAsync("GameAlertsUnitTests\\MlbClient\\MlbResponse.json");
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var mlbResponse = JsonSerializer.Deserialize<MlbResponse>(json, options);
        
        mlbResponse?.TotalGames.ShouldBe(15);
    }
}