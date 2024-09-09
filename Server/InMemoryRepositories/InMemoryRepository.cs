using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly List<T> entities = new();

    public InMemoryRepository()
    {
        AddDummyData();
    }

    private void AddDummyData()
    {
        if (typeof(T) == typeof(Post))
        {
            entities.Add((T)(IEntity)new Post { Id = 1, Title = "Post1", Body = "first post" });
            entities.Add((T)(IEntity)new Post { Id = 2, Title = "Post2", Body = "second post." });
        }
        else if (typeof(T) == typeof(User))
        {
            entities.Add((T)(IEntity)new User { Id = 1, Username = "John Doe", Password = "1234"});
            entities.Add((T)(IEntity)new User { Id = 2, Username = "Jane Doe", Password = "1234" });
        }
        else if (typeof(T) == typeof(Comment))
        {
            entities.Add((T)(IEntity)new Comment { Id = 1, PostId = 1, Body = "this is a comment" });
            entities.Add((T)(IEntity)new Comment { Id = 2, PostId = 1, Body = "this is also a comment" });
        }
    }

    public Task<T> AddAsync(T entity)
    {
        entity.Id = entities.Any() 
            ? entities.Max(e => e.Id) + 1
            : 1; 
        entities.Add(entity);
        
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(T entity)
    {
        var existingEntity = entities.SingleOrDefault(e => e.Id == entity.Id);
        if (existingEntity is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{entity.Id}' not found");
        }

        entities.Remove(existingEntity);
        entities.Add(entity);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var entityToRemove = entities.SingleOrDefault(e => e.Id == id);

        if (entityToRemove is null)
        {
            throw new InvalidOperationException(
                $"Entity with ID '{id}' not found");
        }
        
        entities.Remove(entityToRemove);
        return Task.CompletedTask;  
    }

    public Task<T> GetSingleAsync(int id)
    {
        var entity = entities.SingleOrDefault(e => e.Id == id);

        if (entity is null)
        {
            throw new InvalidOperationException(
                $"Entity with ID '{id}' not found");
        }
 
        return Task.FromResult(entity);  
    }

    public IQueryable<T> GetManyAsync()
    {
        return entities.AsQueryable();
    }
}