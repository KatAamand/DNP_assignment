using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileIORepositories;


public class FileRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly string filePath; 

    public FileRepository(string filePath)
    {
        this.filePath = filePath;
        
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<T> AddAsync(T entity)
    {
        var entities = await ReadFromFileAsync();
        entity.Id = entities.Any() ? entities.Max(e => e.Id) + 1 : 1; 
        entities.Add(entity);
        await WriteToFileAsync(entities);
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        var entities = await ReadFromFileAsync();
        var entityToUpdate = entities.FirstOrDefault(e => e.Id == entity.Id);

        if (entityToUpdate != null)
        {
            entities.Remove(entityToUpdate);
            entities.Add(entity);
            await WriteToFileAsync(entities);
        }
        else
        {
            throw new KeyNotFoundException($"entity with id: {entity.Id} not found");
        }
    }

    public async Task DeleteAsync(int id)
    {
        var entities = await ReadFromFileAsync();
        var entityToDelete = entities.SingleOrDefault(e => e.Id == id);
        if (entityToDelete != null)
        {
            entities.Remove(entityToDelete);
            await WriteToFileAsync(entities);
        }
        else
        {
            throw new KeyNotFoundException($"Entity with Id {id} not found");
        }
    }

    public async Task<T> GetSingleAsync(int id)
    {
        var entities = await ReadFromFileAsync();
    
        var entityToReturn = entities.SingleOrDefault(e => e.Id == id);

        if (entityToReturn == null)
        {
            throw new KeyNotFoundException($"Entity with Id {id} not found");
        }
    
        return entityToReturn;
    }


    public async Task<IQueryable<T>> GetManyAsync()
    {
        var entities = await ReadFromFileAsync();
        return entities.AsQueryable();
    }

    public async Task<T> GetSingleAsync(string identifier)
    {
        var entities = await ReadFromFileAsync();
        
        if (typeof(T) == typeof(User))
        {
            var user = entities.OfType<User>().SingleOrDefault(u => u.Username == identifier);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with Username '{identifier}' not found");
            }

            return (T)(IEntity)user;
        }

        throw new NotSupportedException($"GetSingleAsync(string) is not supported for type '{typeof(T).Name}'");
    }
    
    // Count number of entities
    public async Task<int> CountAsync(Func<T, bool> predicate)
    {
        var entities = await ReadFromFileAsync();
        return entities.Count(predicate);
    }


    // Helper functions 
    private async Task<List<T>> ReadFromFileAsync()
    {
        var jsonData = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>();
    }

    private async Task WriteToFileAsync(List<T> entities)
    {
        var jsonData = JsonSerializer.Serialize(entities, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, jsonData);
    }
}