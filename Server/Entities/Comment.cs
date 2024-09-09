﻿namespace Entities;

public class Comment : IEntity
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int AuthorId { get; set; }
    public int PostId { get; set; }

    public Comment(){}
    
    public Comment(int id, string body, int authorId, int postId)
    {
        Id = id;
        Body = body;
        AuthorId = authorId;
        PostId = postId;
    }
}