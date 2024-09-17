using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    IRepository<User> userRepository;
    IRepository<Post> postRepository;
    IRepository<Comment> commentRepository;

    public CliApp(IRepository<User> userRepository, IRepository<Post> postRepository, IRepository<Comment> commentRepository)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        this.commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
    }

    public async Task StartAsync()
    {
        throw new NotImplementedException();
    }
}