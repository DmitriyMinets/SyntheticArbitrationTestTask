using Core.Entities;
using Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using SyntheticArbitration.DataAccess;

namespace DataAccess.Repository;

public class FuturesArbitrageRepository(DataContext context)
    : Repository<FuturesArbitrage>(context), IFuturesArbitrageRepository
{
    public async Task<FuturesArbitrage?> GetLastAsync(string asset)
    {
        return await _context.FuturesArbitrages
            .Where(e => e.Assets == asset)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }
}