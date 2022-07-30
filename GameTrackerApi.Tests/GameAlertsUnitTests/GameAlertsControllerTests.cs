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
        var logger = A.Fake<ILogger<GameAlertsController>>();
        var sut = new GameAlertsController(logger);

        var result = sut.Get();
        
        result.ShouldBeOfType<GameAlert[]>();
    }
}