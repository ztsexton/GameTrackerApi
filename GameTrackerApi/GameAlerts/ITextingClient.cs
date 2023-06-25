namespace GameTrackerApi.GameAlerts;

public interface ITextingClient
{
    Task SendTextAsync(string messageText);
}