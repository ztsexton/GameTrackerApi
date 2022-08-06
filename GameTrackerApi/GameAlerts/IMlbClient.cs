using GameTrackerApi.GameAlerts.Mlb;

namespace GameTrackerApi.GameAlerts;

public interface IMlbClient
{
    Task<MlbResponse> GetGameInfoAsync(string team, DateTime dateTime);
}