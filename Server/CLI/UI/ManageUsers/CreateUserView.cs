using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IRepository<User> repository;

    public CreateUserView(IRepository<User> repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task Show()
    {
        Console.WriteLine("Opret ny bruger");

        // Indtast brugernavn
        Console.Write("Indtast brugernavn: ");
        string? username = Console.ReadLine();
        var users = (await repository.GetManyAsync()).ToList(); 

        while (string.IsNullOrWhiteSpace(username) || users.Any(u => u.Username == username))
        {
            Console.WriteLine("Brugernavn må ikke være tomt eller allerede taget. Prøv igen.");
            username = Console.ReadLine();
        }

        // Indtast password
        Console.Write("Indtast password: ");
        string? password = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Password må ikke være tomt. Prøv igen.");
            password = Console.ReadLine();
        }

        // Opret en ny bruger
        var newUser = new User
        {
            Username = username,
            Password = password
        };

        // Gem brugeren i repository
        await repository.AddAsync(newUser);  

        // Bekræftelse til brugeren
        Console.WriteLine($"Brugeren '{newUser.Username}' blev oprettet.");
    }
}