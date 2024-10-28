namespace ForumApp.Services;

public interface ICommentService
{
    Task<int> GetCommentCountAsync(int postId);
}