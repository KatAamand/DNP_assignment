// See https://aka.ms/new-console-template for more information

using CLI.UI;
using Entities;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Start CLI app");

// Opretter repositories med dummydata
IRepository<User> userRepository = new InMemoryRepository<User>();
IRepository<Post> postRepository = new InMemoryRepository<Post>();
IRepository<Comment> commentRepository = new InMemoryRepository<Comment>();

CliApp cliApp = new CliApp(userRepository, postRepository, commentRepository); 
await cliApp.StartAsync(); 