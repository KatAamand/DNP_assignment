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

    public async Task<UserDTO> AddUserAsync(CreateUserDTO user)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("users", user);
        var response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<UserDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
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