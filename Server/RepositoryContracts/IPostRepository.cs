using Entities;

namespace RepositoryContracts;

public interface IPostRepository
{
    Post Create(Post post);
    void Update(Post post); 
    void Delete(int postId);
    Post GetSingle(int postId); 
    IQueryable<Post> GetAll();
}