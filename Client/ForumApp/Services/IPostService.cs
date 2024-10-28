using DTO;

namespace ForumApp.Services; 

public interface IPostService
{
    public Task <PostDTO> AddPostAsync(CreatePostDTO createPostDto); 
    public Task UpdatePostAsync(int id, PostDTO postDto);
    public Task<bool> DeletePostAsync(int id);
    public Task<PostDTO> GetPostAsync(int id);
    public Task<List<PostDTO>> GetPostsAsync();
}

