using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IRepository<Post> repository;
    private readonly IRepository<User> userRepository;

    public CreatePostView(IRepository<Post> repository, IRepository<User> userRepository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task Show()
    {
        Console.WriteLine("Opret ny post");
        
        Console.Write("Indtast titel: ");
        string? title = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Titel må ikke være tom, prøv igen");
            title = Console.ReadLine();
        }
        
        Console.Write("Indtast brugerID: ");
        int authorId;
        var users = (await userRepository.GetManyAsync()).ToList(); 

        while (!int.TryParse(Console.ReadLine(), out authorId) || !users.Any(u => u.Id == authorId))
        {
            Console.WriteLine("Ugyldigt brugerID. Prøv igen.");
        }

        
        Console.Write("Indtast indhold: ");
        string? body = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Indhold må ikke være tomt. Prøv igen.");
            body = Console.ReadLine();
        }
        
        var newPost = new Post(title, authorId, body);
        await repository.AddAsync(newPost);
        
        Console.WriteLine($"Post med titlen '{newPost.Title}' er nu oprettet");
    }
}