namespace DTO;

public class CreateCommentDTO
{
    public string Body { get; set; }
    public int AuthorId { get; set; }
    public int PostId { get; set; }
}