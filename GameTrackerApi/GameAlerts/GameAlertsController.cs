using Microsoft.AspNetCore.Mvc;

namespace GameTrackerApi.GameAlerts;

[ApiController]
[Route("[controller]")]
public class GameAlertsController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<GameAlertsController> _logger;
    private readonly IGameAlertsProvider _gameAlertsProvider;

    public GameAlertsController(ILogger<GameAlertsController> logger, IGameAlertsProvider gameAlertsProvider)
    {
        _logger = logger;
        _gameAlertsProvider = gameAlertsProvider;
    }

    [HttpGet(Name = "GetGameAlerts")]
    public IEnumerable<GameAlert> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new GameAlert
            {
                Date = DateTime.Now.AddDays(index),
                HomeTeam = "HomeTeamName",
                AwayTeam = "AwayTeamName"
            })
            .ToArray();
    }

    [HttpGet("{team}")]
    public async Task<IActionResult> GetTeamHomeGameAlert(string team)
    {
        var gameAlert = await _gameAlertsProvider.GetAlertAsync(team, DateTime.Now);
        if (GameAlertHasHomeTeamData(gameAlert))
            return Ok(gameAlert);

        return NotFound();
    }

    private bool GameAlertHasHomeTeamData(GameAlert gameAlert)
    {
        return !string.IsNullOrEmpty(gameAlert.HomeTeam);
    }
}
