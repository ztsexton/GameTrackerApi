using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet("{team}")]
    public async Task<ActionResult> GetTeamHomeGameAlert(string team, DateTime? date)
    {
        if (date is null) date = DateTime.Now;
        var gameAlert = await _gameAlertsProvider.GetAlertAsync(team, date.Value);
        if (GameAlertHasHomeTeamData(gameAlert))
            return Ok(gameAlert.Message);

        return Ok("No home game");
    }


    private bool GameAlertHasHomeTeamData(GameAlert gameAlert)
    {
        return !string.IsNullOrEmpty(gameAlert.HomeTeam);
    }

    [Authorize]
    [HttpGet("nationals")]
    public async Task<IActionResult> GetNationalsHomeGameAlert()
    {
        return await GetTeamHomeGameAlert("Washington Nationals", DateTime.Today);
    }
}
