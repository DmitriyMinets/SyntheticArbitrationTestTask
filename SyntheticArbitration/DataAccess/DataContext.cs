using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<FuturesArbitrage> FuturesArbitrages { get; init; }
}