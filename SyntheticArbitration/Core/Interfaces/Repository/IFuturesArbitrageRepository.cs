using Core.Entities;

namespace Core.Interfaces.Repository;

public interface IFuturesArbitrageRepository : IRepository<FuturesArbitrage>
{
    Task<FuturesArbitrage?> GetLastAsync(string asset);
}