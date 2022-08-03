namespace GameTrackerApi.GameAlerts;

public class GameAlertsProvider : IGameAlertsProvider
{
    public Task<GameAlert> GetAlertAsync(string team, DateTime dateTime)
    {
        GameAlert gameAlert;
        if (string.Equals(team, "Washington Nationals", StringComparison.InvariantCultureIgnoreCase))
            gameAlert = new GameAlert
            {
                HomeTeam = "Washington Nationals"
            };
        else
            gameAlert = new GameAlert();

        return Task.FromResult(gameAlert);
    }
}