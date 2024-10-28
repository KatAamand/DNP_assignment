using Entities;

namespace RepositoryContracts;

public interface IRepository<T>
{
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<T> GetSingleAsync(int id);
    Task<IQueryable<T>> GetManyAsync();
    Task<T> GetSingleAsync(string identifier);
    Task<int> CountAsync(Func<T, bool> predicate);
}