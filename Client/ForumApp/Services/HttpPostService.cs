using System.Text.Json;
using DTO;

namespace ForumApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public HttpPostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    public async Task<PostDTO> AddPostAsync(CreatePostDTO createPostDto)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("posts", createPostDto);

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = await httpResponse.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error: {httpResponse.ReasonPhrase} - {errorMessage}");
        }

        // Use ReadFromJsonAsync for streamlined deserialization
        var response = await httpResponse.Content.ReadFromJsonAsync<PostDTO>(_jsonSerializerOptions);
        if (response == null)
        {
            throw new HttpRequestException("Error: Unable to deserialize the response.");
        }

        return response;
    }

    public async Task UpdatePostAsync(int id, PostDTO postDto)
    {
        HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync($"posts/{id}", postDto);

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = await httpResponse.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error: {httpResponse.ReasonPhrase} - {errorMessage}");
        }
    }

    public async Task<bool> DeletePostAsync(int id)
    {
        HttpResponseMessage httpResponse = await _httpClient.DeleteAsync($"posts/{id}");

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = await httpResponse.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error: {httpResponse.ReasonPhrase} - {errorMessage}");
        }

        return httpResponse.IsSuccessStatusCode;
    }

    public async Task<PostDTO> GetPostAsync(int id)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"posts/{id}");

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = await httpResponse.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error: {httpResponse.ReasonPhrase} - {errorMessage}");
        }

        var response = await httpResponse.Content.ReadFromJsonAsync<PostDTO>(_jsonSerializerOptions);
        if (response == null)
        {
            throw new HttpRequestException("Error: Unable to deserialize the response.");
        }

        return response;
    }

    public async Task<List<PostDTO>> GetPostsAsync()
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync("posts");

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorMessage = await httpResponse.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error: {httpResponse.ReasonPhrase} - {errorMessage}");
        }

        var response = await httpResponse.Content.ReadFromJsonAsync<List<PostDTO>>(_jsonSerializerOptions);
        if (response == null)
        {
            throw new HttpRequestException("Error: Unable to deserialize the response.");
        }

        return response;
    }
}
