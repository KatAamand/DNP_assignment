namespace Entities;

public class Comment : IEntity
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int AuthorId { get; set; }
    public string AuthorUsername { get; set; }
    public int PostId { get; set; }
    public DateTime Created { get; set; }
    
    public Comment(){}
    
    public Comment(string body, int authorId, string authorUsername, int postId)
    {
        Body = body;
        AuthorId = authorId;
        PostId = postId;
        AuthorUsername = authorUsername;
        Created = DateTime.Now;
    }
    
    public Comment(string body, int authorId, int postId)
    {
        Body = body;
        AuthorId = authorId;
        PostId = postId;
        Created = DateTime.Now;
    }
}