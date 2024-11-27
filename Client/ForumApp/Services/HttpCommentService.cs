using System.Net;
using System.Text.Json;
using DTO;
using Microsoft.AspNetCore.Components.Authorization;

namespace ForumApp.Services;

public class HttpCommentService : ICommentService
{
    HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly AuthenticationStateProvider _authprovider;

    public HttpCommentService(HttpClient httpClient, AuthenticationStateProvider authProvider)
    {
        _httpClient = httpClient;
        _authprovider = authProvider;

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }
    
    public async Task<CommentDTO> AddCommentAsync(CreateCommentDTO newComment)
    {
        // Get the current user from AuthenticationStateProvider
        var authState = await _authprovider.GetAuthenticationStateAsync();
        var claimsPrincipal = authState.User;

        if (claimsPrincipal.Identity is null || !claimsPrincipal.Identity.IsAuthenticated)
        {
            throw new InvalidOperationException("User is not logged in");
        }
        
        string? username = claimsPrincipal.Identity.Name;

        if (string.IsNullOrEmpty(username))
        {
            throw new InvalidOperationException("Cannot determinate the username of the logged in user");
        }
        
        newComment.AuthorName = username;

        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync($"comments", newComment);

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = await httpResponse.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error {httpResponse.ReasonPhrase}: {errorMessage}");
        }

        var response = await httpResponse.Content.ReadFromJsonAsync<CommentDTO>(_jsonSerializerOptions);
        if (response == null)
        {
            throw new HttpRequestException($"Error {httpResponse.ReasonPhrase}");
        }

        return response;
    }

    public async Task<int> GetCommentCountAsync(int postId)
    {
        var response = await _httpClient.GetAsync($"comments/count/{postId}");
        response.EnsureSuccessStatusCode();

        var count = await response.Content.ReadFromJsonAsync<int>();
        return count;
    }

    public async Task<List<CommentDTO>> GetCommentsAsync(int? postId = null, int? authorId = null)
    {
        // Build the query string based on provided filter parameters
        var queryParameters = new List<string>();
        if (postId.HasValue)
        {
            queryParameters.Add($"postId={postId.Value}");
        }
        if (authorId.HasValue)
        {
            queryParameters.Add($"authorId={authorId.Value}");
        }

        // Construct the URL with query string if any parameters are set
        var url = "comments";
        if (queryParameters.Any())
        {
            url += "?" + string.Join("&", queryParameters);
        }

        // Make the HTTP GET request
        var response = await _httpClient.GetFromJsonAsync<List<CommentDTO>>(url);
        return response ?? new List<CommentDTO>();
    }


}