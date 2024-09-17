using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IRepository<Post> repository;

    public ListPostsView(IRepository<Post> repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
}