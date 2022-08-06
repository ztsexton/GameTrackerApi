﻿using Newtonsoft.Json;

namespace GameTrackerApi.GameAlerts.Mlb;

public class MlbClient : IMlbClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MlbClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<MlbResponse> GetGameInfoAsync(string team, DateTime dateTime)
    {
        var url = GenerateUrl(dateTime);
        var client = _httpClientFactory.CreateClient();
        var result = await client.GetAsync(url);
        var json = await result.Content.ReadAsStringAsync();

        MlbResponse mlbResponse = JsonConvert.DeserializeObject<MlbResponse>(json);

        return mlbResponse;
    }

    private string GenerateUrl(DateTime dateTime)
    {

        var start = dateTime;
        var end = dateTime;

        var url =
            $"http://statsapi.mlb.com/api/v1/schedule/games/?sportId=1&startDate={start.ToString("yyyy-MM-dd")}&endDate={end.ToString("yyyy-MM-dd")}";
        return url;
    }
}