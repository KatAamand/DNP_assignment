namespace DTO;

public class PostDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int NoOfVotes { get; set; }
    public int AuthorId { get; set; }
    public DateOnly Created { get; set; }
}