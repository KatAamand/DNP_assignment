using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly IRepository<User> repository;

    public ManageUsersView(IRepository<User> repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    public async Task Show()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- Brugerstyring ---");
            Console.WriteLine("1. Opret ny bruger");
            Console.WriteLine("2. Se alle brugere");
            Console.WriteLine("3. Opdater bruger");
            Console.WriteLine("4. Slet bruger");
            Console.WriteLine("5. Tilbage til hovedmenu");
            Console.Write("Vælg en handling (1-5): ");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    var createUserView = new CreateUserView(repository);
                    createUserView.Show();
                    break;
                case "2":
                    var listUserView = new ListUserView(repository);
                    listUserView.Show();
                    break;
                case "3":
                    await UpdateUser(); 
                    break;
                case "4":
                    await DeleteUser(); 
                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Ugyldigt valg, prøv igen.");
                    break;
            }
        }
    }

    private async Task UpdateUser()
    {
        Console.Write("Indtast bruger-ID for den bruger, du vil opdatere: ");
        if (int.TryParse(Console.ReadLine(), out int userId))
        {
            var user = await repository.GetSingleAsync(userId);
            if (user != null)
            {
                Console.Write("Indtast nyt brugernavn (eller tryk Enter for at beholde det nuværende): ");
                string? newUsername = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newUsername))
                {
                    user.Username = newUsername;
                }

                Console.Write("Indtast nyt password (eller tryk Enter for at beholde det nuværende): ");
                string? newPassword = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    user.Password = newPassword;
                }

                repository.UpdateAsync(user);
                Console.WriteLine("Bruger opdateret.");
            }
            else
            {
                Console.WriteLine("Bruger ikke fundet.");
            }
        }
        else
        {
            Console.WriteLine("Ugyldigt bruger-ID.");
        }
    }

    private async Task DeleteUser()
    {
        Console.Write("Indtast bruger-ID for den bruger, du vil slette: ");
        if (int.TryParse(Console.ReadLine(), out int userId))
        {
            var user = repository.GetSingleAsync(userId);
            if (user != null)
            {
                repository.DeleteAsync(userId);
                Console.WriteLine($"Bruger med ID {userId} blev slettet.");
            }
            else
            {
                Console.WriteLine("Bruger ikke fundet.");
            }
        }
        else
        {
            Console.WriteLine("Ugyldigt bruger-ID.");
        }
    }
}