using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ViewSinglePost
{
    private readonly IRepository<Post> postRepository;
    private readonly IRepository<Comment> commentRepository;
    private readonly IRepository<User> userRepository;

    public ViewSinglePost(IRepository<Post> postRepository, IRepository<Comment> commentRepository, IRepository<User> userRepository)
    {
        this.postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        this.commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task showPostAndMenu(int postId)
{
    Post postToShow = await postRepository.GetSingleAsync(postId);
    List<Comment> allComments = (await commentRepository.GetManyAsync()).ToList();  
    List<Comment> commentsForPost = allComments.Where(c => c.PostId == postId).ToList();
    
    if (postToShow == null)
    {
        Console.WriteLine("Ugyldigt postID");
    }
    else
    {
        // Vis posten og kommentarer én gang, når brugeren går ind på ViewSinglePost
        await showPost(postToShow, commentsForPost);

        // Kald menuen uden at vise posten igen
        await getActionMenu(postToShow);  
    }
}

private async Task getActionMenu(Post post)
{
    bool stayInMenu = true;
    List<Comment> commentsForPost = (await commentRepository.GetManyAsync())
        .Where(c => c.PostId == post.Id).ToList();

    while (stayInMenu)
    {

        Console.WriteLine("");
        Console.WriteLine("Hvad vil du nu?");
        Console.WriteLine("1. Stem på post");
        Console.WriteLine("2. Tilføj kommentar");
        Console.WriteLine("3. Rediger post titel");
        Console.WriteLine("4. Slet post");
        Console.WriteLine("5. Gå tilbage");

        Console.Write("Indtast valg: ");
        string? filterChoice = Console.ReadLine();

        switch (filterChoice)
        {
            case "1":
                voteForPost(post); 
                break;
            case "2":
                await addCommentToPost(post); 
                break;
            case "3":
                await editPost(post); 
                break;
            case "4":
                await deletePost(post); 
                stayInMenu = false; // Forlad menuen, efter posten er slettet
                break;
            case "5":
                stayInMenu = false; // Forlad menuen og gå tilbage til hovedmenuen
                break;
            default:
                Console.WriteLine("Ugyldigt valg. Prøv igen.");
                break;
        }

        // Opdater kommentarer efter en handling
        commentsForPost = (await commentRepository.GetManyAsync())
            .Where(c => c.PostId == post.Id).ToList();

        // Hvis der stadig er i menuen, vis kun posten og kommentarer efter en relevant handling
        if (stayInMenu)
        {
            await showPost(post, commentsForPost); // Opdater visningen af posten og kommentarer
            Console.WriteLine("\nTryk på en tast for at fortsætte...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}



    private async Task showPost(Post postToShow, List<Comment> commentsForPost)
    {
        Console.WriteLine("Title: " + postToShow.Title);
        Console.WriteLine("Author: " + postToShow.AuthorId);
        Console.WriteLine(postToShow.Body);
        Console.WriteLine("no of votes: " + postToShow.NoOfVotes);
        Console.WriteLine();

        if (commentsForPost.Any())
        {
            Console.WriteLine("Comments:");
            foreach (var comment in commentsForPost)
            {
                Console.WriteLine(comment.Body);  
            }
        }
    }

    private async Task deletePost(Post post)
    {
        Console.WriteLine("Bekræft sletning af post: 1 for ja, 2 for nej");
        
        string? answer = Console.ReadLine();
        if (answer == "1")
        {
            await postRepository.DeleteAsync(post.Id);
            Console.WriteLine("Posten er blevet slettet.");
        }
        else
        {
            await getActionMenu(post);
        }
    }

    private async Task editPost(Post post)
    {
        Console.WriteLine("Hvad ønsker du at ændre?");
        Console.WriteLine("1. Titel");
        Console.WriteLine("2. Indhold");
        Console.WriteLine("3. Gå tilbage");

        Console.Write("Indtast valg: ");
        string? editChoice = Console.ReadLine();

        switch (editChoice)
        {
            case "1":
                await editPostTitle(post);
                break;
            case "2":
                await editPostBody(post);
                break;
            case "3":
                await getActionMenu(post);
                break;
            default:
                Console.WriteLine("Ugyldigt valg.");
                break;
        }
    }

    private async Task addCommentToPost(Post post)
    {
        Console.Write("Indtast dit brugerId: ");
        if (!int.TryParse(Console.ReadLine(), out int authorId))
        {
            Console.WriteLine("Ugyldigt brugerId. Indtast et gyldigt tal.");
            return;
        }

        var users = (await userRepository.GetManyAsync()).ToList();
        var user = users.SingleOrDefault(u => u.Id == authorId);

        if (user == null)
        {
            Console.WriteLine("Bruger med det indtastede ID findes ikke. Prøv igen.");
            return;
        }

        Console.Write("Indtast din kommentar: ");
        string? commentBody = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(commentBody))
        {
            var newComment = new Comment(commentBody, authorId, post.Id);
            await commentRepository.AddAsync(newComment);

            Console.WriteLine("Din kommentar er blevet tilføjet.");
        }
        else
        {
            Console.WriteLine("Kommentaren må ikke være tom. Prøv igen.");
        }
    }

    private void voteForPost(Post post)
    {
        post.NoOfVotes++; 
        Console.WriteLine($"Posten har nu {post.NoOfVotes} stemmer.");
    }
    
    private async Task editPostTitle(Post post)
    {
        Console.Write("Indtast ny titel: ");
        string? newTitle = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(newTitle))
        {
            post.Title = newTitle;
            await postRepository.UpdateAsync(post);
            Console.WriteLine("Titel opdateret.");
        }
        else
        {
            Console.WriteLine("Titel må ikke være tom.");
        }
    }

    private async Task editPostBody(Post post)
    {
        Console.Write("Indtast nyt indhold: ");
        string? newBody = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(newBody))
        {
            post.Body = newBody;
            await postRepository.UpdateAsync(post);
            Console.WriteLine("Indhold opdateret.");
        }
        else
        {
            Console.WriteLine("Indhold må ikke være tomt.");
        }
    }
}
