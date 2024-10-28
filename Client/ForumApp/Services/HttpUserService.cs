using System.Net;
using System.Text.Json;
using DTO;

namespace ForumApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient _httpClient;

    public HttpUserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> AddUserAsync(string username, string password)
    {
        CreateUserDTO newUser = new CreateUserDTO
        {
            Username = username,
            Password = password
        }; 
        
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("users", newUser);

        if (httpResponse.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            string responseContent = await httpResponse.Content.ReadAsStringAsync();
            Console.Error.WriteLine($"Failed to create user: {httpResponse.StatusCode} - {responseContent}");
            return false;
        }
    }

    public async Task UpdateUserAsync(int id, UserDTO user)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDTO> GetUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<UserDTO>> GetUsersAsync()
    {
        throw new NotImplementedException();
    }
}