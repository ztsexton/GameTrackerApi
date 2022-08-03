namespace GameTrackerApi.GameAlerts;

public interface IGameAlertsProvider
{
    Task<GameAlert> GetAlertAsync(string team, DateTime dateTime);
}