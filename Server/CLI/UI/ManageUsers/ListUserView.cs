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

    public void Show()
    {
        throw new NotImplementedException();
    }
}