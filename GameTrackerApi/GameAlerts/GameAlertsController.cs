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

    public GameAlertsController(ILogger<GameAlertsController> logger)
    {
        _logger = logger;
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
    public IActionResult GetTeamHomeGameAlert(string team)
    {
        if (team == "Washington Nationals")
            return Ok(new GameAlert
            {
                HomeTeam = "Washington Nationals"
            });

        return NotFound();
    }
}