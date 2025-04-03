using Core.Entities;

namespace Core.Interfaces;

public interface IBinanceService
{
    Task ProcessFuturePricesAsync(string symbol1, string symbol2);
    Task<IReadOnlyList<FuturesArbitrage>> GetFuturesArbitrageAsync();
}