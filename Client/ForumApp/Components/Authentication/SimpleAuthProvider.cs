using System.Security.Claims;
using System.Text.Json;
using DTO;
using ForumApp.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace ForumApp.Components.Authentication;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private UserSessionService _userSessionService;

    public SimpleAuthProvider(HttpClient httpClient, UserSessionService userSessionService)
    {
        _httpClient = httpClient;
        _userSessionService = userSessionService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await _userSessionService.InitializeAsync();
        var claimsPrincipal = _userSessionService.CurrentUser != null ? BuildClaimsPrincipal(_userSessionService.CurrentUser) : new ClaimsPrincipal();
        return new AuthenticationState(claimsPrincipal);
    }

    public async Task<bool> Login(string username, string password)
    {
        var loginResponse = await SendLoginRequestAsync(username, password);
        
        if (loginResponse?.UserDto == null)
        {
            throw new HttpRequestException($"Failed to login: invalid login response");
        }

        UserDTO userDTO = loginResponse.UserDto;
        await _userSessionService.SetCurrentUserAsync(userDTO);
        
        var claimsPrincipal = BuildClaimsPrincipal(userDTO);
        NotifyAuthenticationStateChanged((Task.FromResult(new AuthenticationState(claimsPrincipal))));
        
        return true;
    }

    public async Task Logout()
    {
        await _userSessionService.ClearCurrentUserAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
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