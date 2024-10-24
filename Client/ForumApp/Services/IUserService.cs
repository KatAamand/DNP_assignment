using DTO;

namespace ForumApp.Services;

public interface IUserService
{
    public Task<UserDTO> AddUserAsync(CreateUserDTO user);
    public Task UpdateUserAsync(int id, UserDTO user);
    public Task<bool> DeleteUserAsync(int id);
    public Task<UserDTO> GetUserAsync(int id);
    public Task<List<UserDTO>> GetUsersAsync();
}