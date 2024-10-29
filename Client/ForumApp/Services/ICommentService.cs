using DTO;
using ForumApp.Components.BitsAndBobs;

namespace ForumApp.Services;

public interface ICommentService
{
    Task<int> GetCommentCountAsync(int postId);
    public Task<List<CommentDTO>> GetCommentsAsync(int? postId = null, int? authorId = null);
    Task<CommentDTO> AddCommentAsync(CreateCommentDTO newComment);
}