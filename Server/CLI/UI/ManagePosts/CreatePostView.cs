using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IRepository<Post> repository;

    public CreatePostView(IRepository<Post> repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
}