using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    User Create(User user);
    void Update(User user); 
    void Delete(int userId);
    User GetSingle(int userId); 
    IQueryable<User> GetAll();
}