namespace DTO;

public class CommentDTO
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int AuthorId { get; set; }
    public string AuthorUsername { get; set; }
    public int PostId { get; set; }
    public DateTime Created { get; set; }
}