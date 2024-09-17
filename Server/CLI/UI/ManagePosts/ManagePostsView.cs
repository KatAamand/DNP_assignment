using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private readonly IRepository<Post> repository;

    public ManagePostsView(IRepository<Post> repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
}