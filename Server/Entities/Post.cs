namespace Entities;

public class Post : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int NoOfLikes { get; set; }
    public int AuthorId { get; set; }
    
    public Post(){}

    public Post(string title, string body, int authorId)
    {
        Title = title;
        Body = body;
        AuthorId = authorId;
        NoOfLikes = 0; 
    }
}