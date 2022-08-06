namespace GameTrackerApi.GameAlerts;

public class GameAlertsProvider : IGameAlertsProvider
{
    private readonly IMlbClient _mlbClient;

    public GameAlertsProvider(IMlbClient mlbClient)
    {
        _mlbClient = mlbClient;
    }

    public async Task<GameAlert> GetAlertAsync(string team, DateTime dateTime)
    {
        GameAlert gameAlert;
        
        var mlbResponse = await _mlbClient.GetGameInfoAsync(team, dateTime);
        if (mlbResponse.dates is null) return new GameAlert();
        var homeGame = mlbResponse?.dates[0]?.games?.Where(x => x.teams.home.team.name.Equals(team, StringComparison.InvariantCultureIgnoreCase)).ToList();

        if (homeGame is not null && homeGame.Count > 0)
        {
            gameAlert = new GameAlert();
            var homeTeam = homeGame[0].teams.home.team.name;
            if (homeGame[0].teams.home.team.name.Equals(team, StringComparison.InvariantCultureIgnoreCase))
                gameAlert.HomeTeam = homeTeam;
        }
        else
            gameAlert = new GameAlert();

        return gameAlert;
    }
}