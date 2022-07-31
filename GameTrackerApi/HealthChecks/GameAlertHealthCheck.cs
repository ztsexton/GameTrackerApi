using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GameTrackerApi.HealthChecks;

public class GameAlertHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult(
            HealthCheckResult.Healthy("A healthy result."));
    }
}
