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
}