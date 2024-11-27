using System.Security.Claims;
using System.Text.Json;
using DTO;
using ForumApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ForumApp.Components.Authentication;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private UserDTO? _primaryUserCache; 
    
    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }


    public async Task<bool> Login(string username, string password)
    {
        var loginResponse = await SendLoginRequestAsync(username, password);
        
        if (loginResponse?.UserDto == null)
        {
            throw new HttpRequestException($"Failed to login: invalid login response");
        }

        _primaryUserCache = loginResponse.UserDto;
        string serialisedData = JsonSerializer.Serialize(_primaryUserCache);
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
        
        var claimsPrincipal = BuildClaimsPrincipal(_primaryUserCache);
        
        NotifyAuthenticationStateChanged((Task.FromResult(new AuthenticationState(claimsPrincipal))));
        
        return true;
    }

    public async Task Logout()
    {
        _primaryUserCache = null; 
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
    }
    
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_primaryUserCache == null)
        {
            string userAsJson = "";

            try
            {
                userAsJson = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
            }
            catch (InvalidOperationException e)
            {
                return new AuthenticationState(new()); 
            }

            if (string.IsNullOrEmpty(userAsJson))
            {
                return new AuthenticationState(new());
            }
        
            _primaryUserCache = JsonSerializer.Deserialize<UserDTO>(userAsJson);
        }
        
        ClaimsPrincipal claimsPrincipal = BuildClaimsPrincipal(_primaryUserCache);
        return new AuthenticationState(claimsPrincipal);
    }
    
    
    private async Task<LoginResponseDTO?> SendLoginRequestAsync(string username, string password)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("auth/login", new LoginRequestDTO(username, password));

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to login: {httpResponse.ReasonPhrase}");
        }

        var content = await httpResponse.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<LoginResponseDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
    
    private ClaimsPrincipal BuildClaimsPrincipal(UserDTO userDTO)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, userDTO.Username),
            new ("Id", userDTO.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, "apiauth");
        
        return new ClaimsPrincipal(identity);
    }
    
    
}