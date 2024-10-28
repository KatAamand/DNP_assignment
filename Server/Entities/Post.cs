namespace Entities;

public class Post : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int NoOfVotes { get; set; }
    public int AuthorId { get; set; }
    public DateTime Created { get; set; }
    
    public Post(){}

    public Post(string title, int authorId, string body)
    {
        Title = title;
        Body = body;
        AuthorId = authorId;
        NoOfVotes = 0; 
        Created = DateTime.Now; 
    }

}