using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repository;

namespace Application.Services;

public class BinanceService(IBinanceClient binanceClient, IFuturesArbitrageRepository futuresArbitrageRepository)
    : IBinanceService
{
    public async Task ProcessFuturePricesAsync(string symbol1, string symbol2)
    {
        var asset = ValidateAndExtractAsset(symbol1);
        _ = ValidateAndExtractAsset(symbol2);

        var (quarterlyFutures, biQuarterlyFutures) = await GetCandlesticksAsync(symbol1, symbol2);

        (quarterlyFutures, biQuarterlyFutures) =
            await HandleFuturesAsync(asset, quarterlyFutures, biQuarterlyFutures);

        if (quarterlyFutures == null && biQuarterlyFutures == null) return;

        var futuresArbitrage = CreateFuturesArbitrage(in asset, quarterlyFutures, biQuarterlyFutures);

        await futuresArbitrageRepository.AddAsync(futuresArbitrage);
        await futuresArbitrageRepository.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<FuturesArbitrage>> GetFuturesArbitrageAsync()
    {
        return await futuresArbitrageRepository.GetAllAsync();
    }

    private static FuturesArbitrage CreateFuturesArbitrage(in string asset, BinanceCandleDto quarterlyFutures,
        BinanceCandleDto biQuarterlyFutures)
    {
        return new FuturesArbitrage()
        {
            Assets = asset,
            QuarterPrice = quarterlyFutures.Close,
            BiQuarterPrice = biQuarterlyFutures.Close,
            PriceDifference = biQuarterlyFutures.Close - quarterlyFutures.Close,
            CreatedAt = DateTime.UtcNow
        };
    }

    private async Task<(BinanceCandleDto? quarterlyFutures, BinanceCandleDto? biQuarterlyFutures)> GetCandlesticksAsync(
        string symbol1, string symbol2)
    {
        var candles1Task = binanceClient.GetCandlesticksAsync(symbol1);
        var candles2Task = binanceClient.GetCandlesticksAsync(symbol2);
        var results = await Task.WhenAll(candles1Task, candles2Task);

        var quarterlyFutures = results[0].Count > 0 ? results[0][0] : null;
        var biQuarterlyFutures = results[1].Count > 0 ? results[1][0] : null;

        return (quarterlyFutures, biQuarterlyFutures);
    }


    private async Task<(BinanceCandleDto? quarterlyFutures, BinanceCandleDto? biQuarterlyFutures)>
        HandleFuturesAsync(string asset, BinanceCandleDto? quarterlyFutures, BinanceCandleDto? biQuarterlyFutures)
    {
        var isQuarterlyOld = IsCandlestickOldOrNull(quarterlyFutures);
        var isBiQuarterlyOld = IsCandlestickOldOrNull(biQuarterlyFutures);

        if (!isQuarterlyOld && !isBiQuarterlyOld) return (quarterlyFutures, biQuarterlyFutures);

        var lastFuturesArbitrage = await futuresArbitrageRepository.GetLastAsync(asset);

        if (lastFuturesArbitrage == null)
        {
            return (null, null);
        }

        if (isQuarterlyOld)
            quarterlyFutures = new BinanceCandleDto()
            {
                Close = lastFuturesArbitrage.QuarterPrice,
            };

        if (isBiQuarterlyOld)
            biQuarterlyFutures = new BinanceCandleDto()
            {
                Close = lastFuturesArbitrage.BiQuarterPrice,
            };

        return (quarterlyFutures, biQuarterlyFutures);
    }

    private static bool IsCandlestickOldOrNull(BinanceCandleDto? candle)
    {
        return candle?.EndTimestamp < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    private static string ValidateAndExtractAsset(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol) || !symbol.Contains('_'))
        {
            throw new ArgumentException("Недопустимый формат символа. Ожидаемый формат: 'ASSET_QUARTERLY'",
                nameof(symbol));
        }

        return symbol.Split('_')[0];
    }
}