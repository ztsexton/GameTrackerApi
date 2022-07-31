using System.Collections.Generic;
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
    public void GetTeamHomeGameAlertReturnsSingleAlert()
    {
        var sut = CreateGameAlertsController();

        var result = sut.GetTeamHomeGameAlert("Washington Nationals");
        
        result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public void GetTeamHomeGameAlertWithNonExistantTeamReturns404()
    {
        var sut = CreateGameAlertsController();

        var result = sut.GetTeamHomeGameAlert("The Non Existent Team");
        
        result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public void GetHomeGameAlertReturnsAlertWithProperHomeTeam()
    {
        var sut = CreateGameAlertsController();

        var actionResult = sut.GetTeamHomeGameAlert("Washington Nationals");
        var objectResult = actionResult as OkObjectResult;
        var gameAlert = objectResult.Value as GameAlert;
        
        gameAlert.HomeTeam.ShouldBe("Washington Nationals");
        
    }

    private GameAlertsController CreateGameAlertsController()
    {
        var logger = A.Fake<ILogger<GameAlertsController>>();
        var sut = new GameAlertsController(logger);

        return sut;
    }
}