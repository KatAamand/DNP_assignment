using Entities;

namespace InMemoryRepositories;

public class Test
{
    public static void Main(string[] args)
    {
        // Opret repositories med dummydata
        var postRepository = new InMemoryRepository<Post>();
        var userRepository = new InMemoryRepository<User>();
        var commentRepository = new InMemoryRepository<Comment>();

        // Hent og udskriv dummydata for posts
        var posts = postRepository.GetManyAsync();
        Console.WriteLine("Posts:");
        foreach (var post in posts)
        {
            Console.WriteLine($"ID: {post.Id}, Title: {post.Title}, Body: {post.Body}");
        }

        Console.WriteLine();

        // Hent og udskriv dummydata for users
        var users = userRepository.GetManyAsync();
        Console.WriteLine("Users:");
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id}, Username: {user.Username}, Password: {user.Password}");
        }

        Console.WriteLine();

        // Hent og udskriv dummydata for comments
        var comments = commentRepository.GetManyAsync();
        Console.WriteLine("Comments:");
        foreach (var comment in comments)
        {
            Console.WriteLine($"ID: {comment.Id}, Post ID: {comment.PostId}, Body: {comment.Body}");
        }
    }
}