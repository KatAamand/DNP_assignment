namespace ForumApp.Services;

public class HttpCommentService : ICommentService
{
    HttpClient _httpClient;

    public HttpCommentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetCommentCountAsync(int postId)
    {
        var response = await _httpClient.GetAsync($"comments/count/{postId}");
        response.EnsureSuccessStatusCode();

        var count = await response.Content.ReadFromJsonAsync<int>();
        return count;
    }
}