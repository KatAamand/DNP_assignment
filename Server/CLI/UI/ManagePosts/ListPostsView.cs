using Entities;
using RepositoryContracts;

public class ListPostsView
{
    private readonly IRepository<Post> postRepository;

    public ListPostsView(IRepository<Post> postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task ShowFilterMenu()
    {
        Console.WriteLine("Vælg en filtreringsmulighed:");
        Console.WriteLine("1. Dagens posts");
        Console.WriteLine("2. Posts fra en bestemt dato");
        Console.WriteLine("3. Posts fra en bestemt bruger");
        Console.WriteLine("4. Alle posts");

        Console.Write("Indtast valg: ");
        string? filterChoice = Console.ReadLine();

        switch (filterChoice)
        {
            case "1":
                await Show(onlyToday: true);
                break;
            case "2":
                await ShowPostsByDate();
                break;
            case "3":
                await ShowPostsByUser();
                break;
            case "4":
                await Show(onlyToday: false);
                break;
            default:
                Console.WriteLine("Ugyldigt valg. Prøv igen.");
                break;
        }
    }

    public async Task Show(bool onlyToday)
    {
        var posts = (await postRepository.GetManyAsync()).ToList();

        if (onlyToday)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            posts = posts.Where(p => p.Created == today).ToList(); 
        }

        DisplayPosts(posts, onlyToday ? "Dagens posts:" : "Alle posts:");
    }

    public async Task ShowPostsByDate()
    {
        Console.Write("Indtast dato (åååå-mm-dd): ");
        string? dateInput = Console.ReadLine();

        if (DateOnly.TryParse(dateInput, out DateOnly selectedDate))
        {
            var posts = (await postRepository.GetManyAsync()).Where(p => p.Created == selectedDate).ToList();
            DisplayPosts(posts, $"Posts fra {selectedDate}:");
        }
        else
        {
            Console.WriteLine("Ugyldig datoformat. Prøv igen.");
        }
    }

    public async Task ShowPostsByUser()
    {
        Console.Write("Indtast bruger-ID: ");
        if (int.TryParse(Console.ReadLine(), out int userId))
        {
            var posts = (await postRepository.GetManyAsync()).Where(p => p.AuthorId == userId).ToList();
            DisplayPosts(posts, $"Posts fra bruger med ID {userId}:");
        }
        else
        {
            Console.WriteLine("Ugyldigt bruger-ID. Prøv igen.");
        }
    }

    private void DisplayPosts(List<Post> posts, string header)
    {
        if (posts.Any())
        {
            Console.WriteLine(header);
            foreach (var post in posts)
            {
                Console.WriteLine($"ID: {post.Id}, Titel: {post.Title}, AuthorID: {post.AuthorId}, noOfVotes {post.NoOfVotes}");
                Console.WriteLine(post.Body);
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("Ingen posts fundet.");
        }
    }
}
