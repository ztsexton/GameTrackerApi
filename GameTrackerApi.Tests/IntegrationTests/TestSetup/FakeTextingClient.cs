using System.Threading.Tasks;
using GameTrackerApi.GameAlerts;

namespace GameTrackerApi.Tests.IntegrationTests.TestSetup;

public class FakeTextingClient : ITextingClient
{
    public Task SendTextAsync(string messageText)
    {
        return Task.CompletedTask;
    }
}