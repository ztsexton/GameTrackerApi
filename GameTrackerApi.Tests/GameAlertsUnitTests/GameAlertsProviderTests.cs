using System;
using System.Threading.Tasks;
using GameTrackerApi.GameAlerts;
using Shouldly;
using Xunit;

namespace GameTrackerApi.Tests.GameAlertsUnitTests;

public class GameAlertsProviderTests
{
    [Fact]
    public void GameAlertsProviderCanBeCreated()
    {
        var sut = new GameAlertsProvider();
    }

    [Fact]
    public async Task GameAlertsProviderReturnsValidGameAlertTypeWithExistingTeam()
    {
        var sut = new GameAlertsProvider();

        var result = await sut.GetAlertAsync("Washington Nationals", DateTime.Now);

        result.ShouldBeOfType<GameAlert>();
    }
    
    [Fact]
    public async Task GameAlertsProviderReturnsValidGameAlertWithExistingTeam()
    {
        var sut = new GameAlertsProvider();

        var result = await sut.GetAlertAsync("Washington Nationals", DateTime.Now);

        result.HomeTeam.ShouldBe("Washington Nationals");
    }
}