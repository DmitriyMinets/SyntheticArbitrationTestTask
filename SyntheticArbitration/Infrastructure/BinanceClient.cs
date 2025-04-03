using System.Text.Json;
using Core.Const;
using Core.DTOs;
using Core.Interfaces;
using Infrastructure.Extensions;
using Infrastructure.Helpers;

namespace Infrastructure;

public class BinanceClient(HttpClient httpClient) : IBinanceClient
{
    private const string BinanceKlinesUrl = "https://fapi.binance.com/fapi/v1/klines?symbol=";

    public async Task<IReadOnlyList<BinanceCandleDto>> GetCandlesticksAsync(string symbol)
    {
        return await GetCandlesticksAsync(symbol, Interval.OneHour, 1);
    }

    public async Task<IReadOnlyList<BinanceCandleDto>> GetCandlesticksAsync(string symbol, Interval interval,
        int limit)
    {
        var url = $"{BinanceKlinesUrl}{symbol}&interval={interval.ToStringValue()}&limit={limit}";
        var result = await httpClient.GetStringAsync(url);

        var jsonElements = JsonSerializer.Deserialize<List<List<JsonElement>>>(result);

        return jsonElements?.Select(c => new BinanceCandleDto
        {
            Close = c[4].GetDecimalInvariant(),
            EndTimestamp = c[6].GetInt64(),
        }).ToList() ?? [];
    }
}