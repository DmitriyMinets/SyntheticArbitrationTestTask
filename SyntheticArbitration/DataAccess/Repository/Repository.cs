using Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using SyntheticArbitration.DataAccess;

namespace DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DataContext _context;
    protected Repository(DataContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }
}