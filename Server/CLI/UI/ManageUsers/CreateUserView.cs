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
}