namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int AuthorId { get; set; }

    public Comment(int id, string body, int authorId)
    {
        Id = id;
        Body = body;
        AuthorId = authorId;
    }
}