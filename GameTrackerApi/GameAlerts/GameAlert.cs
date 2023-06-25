namespace GameTrackerApi.GameAlerts;

public class GameAlert
{
    public DateTime Date { get; set; }
    public string HomeTeam { get; set; }
    public string AwayTeam { get; set; }
    public string Message { get; set; }
}
