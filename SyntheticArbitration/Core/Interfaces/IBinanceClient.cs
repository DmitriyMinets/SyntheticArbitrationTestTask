using Core.Const;
using Core.DTOs;

namespace Core.Interfaces;

public interface IBinanceClient
{
    Task<IReadOnlyList<BinanceCandleDto>> GetCandlesticksAsync(string symbol);
    Task<IReadOnlyList<BinanceCandleDto>> GetCandlesticksAsync(string symbol, Interval interval, int limit);
}