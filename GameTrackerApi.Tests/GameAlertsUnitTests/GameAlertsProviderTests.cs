using System;
using System.Threading.Tasks;
using AutoFixture;
using FakeItEasy;
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
        var textingClient = CreateFakeTextingClient();
        var sut = new GameAlertsProvider(mlbClient, textingClient);
    }

    private ITextingClient CreateFakeTextingClient()
    {
        var textingClient = A.Fake<ITextingClient>();
        return textingClient;
    }

    [Fact]
    public async Task GameAlertsProviderReturnsValidGameAlertTypeWithExistingTeam()
    {
        var mlbClient = CreateFakeMlbClientWithHomeTeamResponse();
        var textingClient = CreateFakeTextingClient();
        var sut = new GameAlertsProvider(mlbClient, textingClient);

        var result = await sut.GetAlertAsync("Washington Nationals", DateTime.Now);

        result.ShouldBeOfType<GameAlert>();
    }
    
    [Fact]
    public async Task GameAlertsProviderReturnsValidGameAlertWithExistingTeam()
    {
        var mlbClient = CreateFakeMlbClientWithHomeTeamResponse();
        var textingClient = CreateFakeTextingClient();
        var sut = new GameAlertsProvider(mlbClient, textingClient);

        var result = await sut.GetAlertAsync("Washington Nationals", DateTime.Now);

        result.HomeTeam.ShouldBe("Washington Nationals");
    }

    [Fact]
    public async Task GameAlertsProviderCallsMlbApiWhenMlbTeamIsGiven()
    {
        var mlbClient = CreateFakeMlbClientWithHomeTeamResponse();
        var textingClient = CreateFakeTextingClient();
        var sut = new GameAlertsProvider(mlbClient, textingClient);
        
        var result = await sut.GetAlertAsync("Washington Nationals", DateTime.Now);

        A.CallTo(() => mlbClient.GetGameInfoAsync(A<string>._, A<DateTime>._)).MustHaveHappened();
    }

    [Fact]
    public async Task GameAlertsProviderReturnsEmptyGameAlertIfNoGameFound()
    {
        var mlbClient = A.Fake<IMlbClient>();
        var textingClient = CreateFakeTextingClient();
        var sut = new GameAlertsProvider(mlbClient, textingClient);

        var result = await sut.GetAlertAsync("NonExistentTeam", DateTime.Now);
        
        result.HomeTeam.ShouldBeNull();
    }

    [Fact]
    public async Task GameAlertsProviderSendsATextWhenTeamHasHomeGame()
    {
        var mlbClient = A.Fake<IMlbClient>();
        var textingClient = A.Fake<ITextingClient>();
        var fixture = new Fixture();
        var mlbResponse = fixture.Create<MlbResponse>();
        mlbResponse.Dates[0].Games[0].Teams.Home.Team.Name = "HomeTeam";
        A.CallTo(() => mlbClient.GetGameInfoAsync("HomeTeam", A<DateTime>._)).Returns(mlbResponse);
        
        var sut = new GameAlertsProvider(mlbClient, textingClient);

        var result = await sut.GetAlertAsync("HomeTeam", DateTime.Now);

        A.CallTo(() => textingClient.SendTextAsync(A<string>._)).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task GameAlertsProviderDoesNotSendTextWhenTeamDoesNotHaveHomeGame()
    {
        var mlbClient = A.Fake<IMlbClient>();
        var textingClient = A.Fake<ITextingClient>();
        var fixture = new Fixture();
        var mlbResponse = fixture.Create<MlbResponse>();
        mlbResponse.Dates[0].Games[0].Teams.Away.Team.Name = "AwayTeam";
        A.CallTo(() => mlbClient.GetGameInfoAsync("AwayTeam", A<DateTime>._)).Returns(mlbResponse);
        
        var sut = new GameAlertsProvider(mlbClient, textingClient);

        var result = await sut.GetAlertAsync("AwayTeam", DateTime.Now);

        A.CallTo(() => textingClient.SendTextAsync(A<string>._)).MustNotHaveHappened();
    }

    private IMlbClient CreateFakeMlbClientWithHomeTeamResponse()
    {
        var mlbClient = A.Fake<IMlbClient>();
        var fixture = new Fixture();
        var mlbResponse = fixture.Create<MlbResponse>();
        mlbResponse.Dates[0].Games[0].Teams.Home.Team.Name = "Washington Nationals";
        A.CallTo(() => mlbClient.GetGameInfoAsync(A<string>._, A<DateTime>._)).Returns(mlbResponse);

        return mlbClient;
    }
    
}