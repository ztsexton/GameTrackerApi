using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using FakeItEasy;
using AutoFixture.AutoFakeItEasy;
using GameTrackerApi.GameAlerts;
using GameTrackerApi.GameAlerts.Mlb;
using Shouldly;
using Xunit;

namespace GameTrackerApi.Tests.GameAlertsUnitTests;

public class GameAlertsProviderTests
{
    [Fact]
    public void GameAlertsProviderCanBeCreated()
    {
        var mlbClient = CreateFakeMlbClientWithHomeTeamResponse();
        var sut = new GameAlertsProvider(mlbClient);
    }

    [Fact]
    public async Task GameAlertsProviderReturnsValidGameAlertTypeWithExistingTeam()
    {
        var mlbClient = CreateFakeMlbClientWithHomeTeamResponse();
        var sut = new GameAlertsProvider(mlbClient);

        var result = await sut.GetAlertAsync("Washington Nationals", DateTime.Now);

        result.ShouldBeOfType<GameAlert>();
    }
    
    [Fact]
    public async Task GameAlertsProviderReturnsValidGameAlertWithExistingTeam()
    {
        var mlbClient = CreateFakeMlbClientWithHomeTeamResponse();
        var sut = new GameAlertsProvider(mlbClient);

        var result = await sut.GetAlertAsync("Washington Nationals", DateTime.Now);

        result.HomeTeam.ShouldBe("Washington Nationals");
    }

    [Fact]
    public async Task GameAlertsProviderCallsMlbApiWhenMlbTeamIsGiven()
    {
        var mlbClient = CreateFakeMlbClientWithHomeTeamResponse();
        var sut = new GameAlertsProvider(mlbClient);
        
        var result = await sut.GetAlertAsync("Washington Nationals", DateTime.Now);

        A.CallTo(() => mlbClient.GetGameInfoAsync(A<string>._, A<DateTime>._)).MustHaveHappened();
    }

    [Fact]
    public async Task GameAlertsProviderReturnsEmptyGameAlertIfNoGameFound()
    {
        var mlbClient = A.Fake<IMlbClient>();
        var sut = new GameAlertsProvider(mlbClient);

        var result = await sut.GetAlertAsync("NonExistentTeam", DateTime.Now);
        
        result.HomeTeam.ShouldBeNull();
    }

    private IMlbClient CreateFakeMlbClientWithHomeTeamResponse()
    {
        var mlbClient = A.Fake<IMlbClient>();
        var fixture = new Fixture();
        var mlbResponse = fixture.Create<MlbResponse>();
        mlbResponse.dates[0].games[0].teams.home.team.name = "Washington Nationals";
        A.CallTo(() => mlbClient.GetGameInfoAsync(A<string>._, A<DateTime>._)).Returns(mlbResponse);

        return mlbClient;
    }
    
}