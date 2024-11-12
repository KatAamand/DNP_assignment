namespace ForumApp.Services;
using System.Text.Json;
using DTO;
using Microsoft.JSInterop;

public class UserSessionService
{
    private readonly IJSRuntime _jsRuntime;
    private UserDTO? _currentUser;

    private const string SessionStorageKey = "currentUser";

    public UserSessionService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        // Load the user data from sessionStorage if it exists
        var userData = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", SessionStorageKey);
        if (!string.IsNullOrEmpty(userData))
        {
            _currentUser = JsonSerializer.Deserialize<UserDTO>(userData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }

    public UserDTO? CurrentUser => _currentUser;

    public async Task SetCurrentUserAsync(UserDTO user)
    {
        _currentUser = user;
        var serializedData = JsonSerializer.Serialize(user);
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", SessionStorageKey, serializedData);
    }

    public async Task ClearCurrentUserAsync()
    {
        _currentUser = null;
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", SessionStorageKey);
    }
}
