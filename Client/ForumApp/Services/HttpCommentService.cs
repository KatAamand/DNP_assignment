using System.Net;
using System.Text.Json;
using DTO;

namespace ForumApp.Services;

public class HttpCommentService : ICommentService
{
    HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public HttpCommentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }
    
    public async Task<CommentDTO> AddCommentAsync(CreateCommentDTO newComment)
    {
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