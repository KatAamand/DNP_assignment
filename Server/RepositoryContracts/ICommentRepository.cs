using Entities;

namespace RepositoryContracts;

public interface ICommentRepository
{
    Comment Create(Comment comment);
    void Update(Comment comment); 
    void Delete(int commentId);
    Comment GetSingle(int commentId); 
    IQueryable<Comment> GetAll();
}