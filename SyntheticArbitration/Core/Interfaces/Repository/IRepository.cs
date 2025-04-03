namespace Core.Interfaces.Repository;

public interface IRepository<T> where T : class
{
    Task SaveChangesAsync();
    Task<IReadOnlyList<T>> GetAllAsync();
    Task AddAsync(T entity);
}