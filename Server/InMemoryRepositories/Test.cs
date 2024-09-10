using Entities;

namespace InMemoryRepositories;

public class Test
{
    public static void Main(string[] args)
    {
        // Opretter repositories med dummydata
        var postRepository = new InMemoryRepository<Post>();
        var userRepository = new InMemoryRepository<User>();
        var commentRepository = new InMemoryRepository<Comment>();

        // Henter og udskriver dummy data
        var posts = postRepository.GetManyAsync();
        Console.WriteLine("Posts: ");
        foreach (var post in posts)
        {
            Console.WriteLine($"Id: {post.Id}, Title: {post.Title}, Body: {post.Body}");
        }

        Console.WriteLine();

        var users = userRepository.GetManyAsync();
        Console.WriteLine("Users: ");
        foreach (var user in users)
        {
            Console.WriteLine($"Id: {user.Id}, Username: {user.Username}, Password: {user.Password}");
        }

        Console.WriteLine();

        var comments = commentRepository.GetManyAsync();
        Console.WriteLine("Comments: ");
        foreach (var comment in comments)
        {
            Console.WriteLine($"Id: {comment.Id}, PostID: {comment.PostId}, Body: {comment.Body}");
        }
    }
}