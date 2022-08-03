using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using GameTrackerApi.GameAlerts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit;

namespace GameTrackerApi.Tests.GameAlertsUnitTests;

public class GameAlertsControllerTests
{
    [Fact]
    public void GetReturnsGameAlertArray()
    {
        var sut = CreateGameAlertsController();

        var result = sut.Get();
        
        result.ShouldBeOfType<GameAlert[]>();
    }

    [Fact]
    public async Task GetTeamHomeGameAlertReturnsSingleAlert()
    {
        var gameAlertsProvider = CreateGameAlertProviderForWashingtonNationals();
        var sut = CreateGameAlertsController(gameAlertsProvider);

        var result = await sut.GetTeamHomeGameAlert("Washington Nationals");
        
        result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetTeamHomeGameAlertWithNonExistentTeamReturns404()
    {
        var sut = CreateGameAlertsController();

        var result = await sut.GetTeamHomeGameAlert("The Non Existent Team");
        
        result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetHomeGameAlertReturnsAlertWithProperHomeTeam()
    {
        var gameAlertsProvider = CreateGameAlertProviderForWashingtonNationals();
        var sut = CreateGameAlertsController(gameAlertsProvider);

        var actionResult = await sut.GetTeamHomeGameAlert("Washington Nationals");
        var objectResult = actionResult as OkObjectResult;
        var gameAlert = objectResult?.Value as GameAlert;

        if (gameAlert is null) throw new ArgumentException(nameof(gameAlert));
        gameAlert.HomeTeam.ShouldBe("Washington Nationals");
        
    }

    private GameAlertsController CreateGameAlertsController(IGameAlertsProvider gameAlertsProvider)
    {
        var logger = A.Fake<ILogger<GameAlertsController>>();
        return new GameAlertsController(logger, gameAlertsProvider);
    }
    
    private GameAlertsController CreateGameAlertsController()
    {
        var logger = A.Fake<ILogger<GameAlertsController>>();
        var gameAlertsProvider = A.Fake<IGameAlertsProvider>();
        return new GameAlertsController(logger, gameAlertsProvider);
    }

    private IGameAlertsProvider CreateGameAlertProviderForWashingtonNationals()
    {
        var gameAlertsProvider = A.Fake<IGameAlertsProvider>();
        A.CallTo(() => gameAlertsProvider.GetAlertAsync("Washington Nationals", A<DateTime>._))
            .Returns(new GameAlert { HomeTeam = "Washington Nationals" });

        return gameAlertsProvider;
    }
}
