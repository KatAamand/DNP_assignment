namespace Entities;

public class Comment : IEntity
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int AuthorId { get; set; }
    public int PostId { get; set; }
    
    public Comment(){}
    
    public Comment(string body, int authorId, int postId)
    {
        Body = body;
        AuthorId = authorId;
        PostId = postId;
    }
}