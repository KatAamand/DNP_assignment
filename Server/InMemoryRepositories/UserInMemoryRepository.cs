using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    public List<User> users = new List<User>();


    public Task<User> AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetSingleAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<User> GetMany()
    {
        throw new NotImplementedException();
    }
}