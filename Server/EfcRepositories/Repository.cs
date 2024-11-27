using EfcRepositories;
using Entities;
using Microsoft.EntityFrameworkCore; 

namespace RepositoryContracts;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ForumContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ForumContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<T>();
    }

    public async Task<T> AddAsync(T entity)
    {
        var entityEntry = _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        
        if (entity == null) throw new KeyNotFoundException("Entity with id: " + id + "not found");
        
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<T> GetSingleAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        
        if (entity == null) throw new KeyNotFoundException("Entity with id: " + id + " not found");
        return entity;
    }

    public async Task<IQueryable<T>> GetManyAsync()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<T> GetSingleAsync(Func<T, bool> predicate)
    {
        var entity = await Task.Run(() => _dbSet.AsEnumerable().SingleOrDefault(predicate));

        if (entity == null)
        {
            throw new KeyNotFoundException("Entity matching the predicate was not found.");
        }

        return entity;
    }

    public async Task<int> CountAsync(Func<T, bool> predicate)
    {
        return await Task.FromResult(_dbSet.Count(predicate));
    }
}