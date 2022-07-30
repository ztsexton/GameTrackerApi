using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using GameTrackerApi.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shouldly;
using Xunit;

namespace GameTrackerApi.Tests.HealthChecks;

public class GameAlertsHealthCheck 
{
    [Fact]
    public async Task GetHealthCheckReturnsHealthCheckResult()
    {
        var context = new HealthCheckContext();
        var sut = new GameAlertHealthCheck();

        var result = await sut.CheckHealthAsync(context);

        result.ShouldBeOfType<HealthCheckResult>();
    }
}