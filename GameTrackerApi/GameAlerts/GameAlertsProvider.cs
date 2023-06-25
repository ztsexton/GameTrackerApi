namespace GameTrackerApi.GameAlerts;

public class GameAlertsProvider : IGameAlertsProvider
{
    private readonly IMlbClient _mlbClient;
    private readonly ITextingClient _textingClient;

    public GameAlertsProvider(IMlbClient mlbClient, ITextingClient textingClient)
    {
        _mlbClient = mlbClient;
        _textingClient = textingClient;
    }

    public async Task<GameAlert> GetAlertAsync(string team, DateTime dateTime)
    {
        GameAlert gameAlert;
        
        var mlbResponse = await _mlbClient.GetGameInfoAsync(team, dateTime);
        if (mlbResponse.Dates is null) return new GameAlert();
        var homeGame = mlbResponse?.Dates[0]?.Games?.Where(x => x.Teams.Home.Team.Name.Equals(team, StringComparison.InvariantCultureIgnoreCase)).ToList();

        if (homeGame is not null && homeGame.Count > 0)
        {
            gameAlert = new GameAlert();
            var homeTeam = homeGame[0].Teams.Home.Team.Name;
            if (homeGame[0].Teams.Home.Team.Name.Equals(team, StringComparison.InvariantCultureIgnoreCase))
            {
                var message = $"The {homeTeam} have a home game today at {homeGame[0].GameDate:h:mm tt}";
                gameAlert.HomeTeam = homeTeam;
                gameAlert.Message = message;
                await _textingClient.SendTextAsync(message);
            }
        }
        else
            gameAlert = new GameAlert();

        return gameAlert;
    }
}