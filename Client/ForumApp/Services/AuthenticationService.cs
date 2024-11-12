using System.Text.Json;
using DTO;
using Microsoft.AspNetCore.Identity.Data;

namespace ForumApp.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    public bool IsLoggedIn { get; set; } = false; 
    public int UserId { get; set; }
    public string Username { get; set; }

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        // Check if username and password are not empty
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return false;
        }

        // Create the login request DTO
        var request = new LoginRequestDTO
        {
            Username = username,
            Password = password
        };

        try
        {
            // Send the POST request to the server
            HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("auth/login", request);

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<LoginResponseDTO>();

                if (response != null && response.IsAuthenticated)
                {
                    IsLoggedIn = true;
                    UserId = response.UserDto.Id;
                    Username = response.UserDto.Username;
                    return true;
                }
            }

            // If the response was not successful, log or throw an exception
            var errorContent = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception($"Login failed: {httpResponse.ReasonPhrase} - {errorContent}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Login error: {ex.Message}");
            IsLoggedIn = false;
            UserId = 0;
            Username = string.Empty;
            return false;
        }
    }

    public Task LogoutAsync()
    {
        IsLoggedIn = false;
        UserId = 0;
        return Task.CompletedTask;
    }
}