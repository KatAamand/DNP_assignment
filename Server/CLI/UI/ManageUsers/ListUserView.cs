using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUserView
{
    private readonly IRepository<User> repository;

    public ListUserView(IRepository<User> repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task Show()
    {
        var users = (await repository.GetManyAsync()).ToList(); 

        if (users.Any())
        {
            Console.WriteLine("Liste over brugere:");
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, Brugernavn: {user.Username}");
            }
        }
        else
        {
            Console.WriteLine("Ingen brugere fundet.");
        }
    }
}