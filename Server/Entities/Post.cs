namespace Entities;

public class Post : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int NoOfVotes { get; set; }
    public int AuthorId { get; set; }
    public DateOnly Created { get; set; }
    
    public Post(){}

    public Post(string title, string body, int authorId)
    {
        Title = title;
        Body = body;
        AuthorId = authorId;
        NoOfVotes = 0; 
        Created = DateOnly.FromDateTime(DateTime.Now); 
    }
}