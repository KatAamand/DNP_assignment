namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int NoOfLikes { get; set; }
    public List<Comment> ListOfComments { get; set; }

    public Post(string title, string body)
    {
        Title = title;
        Body = body;
        NoOfLikes = 0; 
        ListOfComments = new List<Comment>();
    }
}