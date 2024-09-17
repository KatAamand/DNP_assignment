﻿using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
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
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("Velkommen til det skæve forum!");
            Console.WriteLine("Hvad skal der ske?");
            Console.WriteLine("1. Se posts");
            Console.WriteLine("2. Opret ny post");
            Console.WriteLine("3. Opret ny bruger");
            Console.WriteLine("4. Se alle brugere");
            Console.WriteLine("5. Afslut");
            
            Console.WriteLine("Indtast valg fra 1 - 5");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    var listPostsView = new ListPostsView(postRepository); // Viser dagens posts
                    listPostsView.ShowFilterMenu();
                    break;
                case "2":
                    var createPostView = new CreatePostView(postRepository); // Opret ny post
                    createPostView.Show();
                    break;
                case "3":
                    var createUserView = new CreateUserView(userRepository); // Opret ny bruger
                    createUserView.Show();
                    break;
                case "4":
                    var listUserView = new ListUserView(userRepository); // Se alle brugere
                    listUserView.Show();
                    break;
                case "5":
                    running = false; // Afslut programmet
                    break;
                default:
                    Console.WriteLine("Ugyldigt valg, prøv igen.");
                    break;
            }
            
        }
    }
}