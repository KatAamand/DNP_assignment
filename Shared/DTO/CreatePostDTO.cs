namespace DTO;

public class CreatePostDTO
{
    public required string Title { get; set; }
    public string Body { get; set; }
    public required int AuthorId { get; set; }
}