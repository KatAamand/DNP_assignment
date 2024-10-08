// See https://aka.ms/new-console-template for more information

using CLI.UI;
using Entities;
using FileIORepositories;
using RepositoryContracts;

Console.WriteLine("Start CLI app");

// json filestrings
string userRepositoryFilepath = "UserRepo.json"; 
string postRepositoryFilepath = "PostRepo.json";
string commentRepositoryFilepath = "CommentRepo.json";

// Opretter repositories med dummydata
IRepository<User> userRepository = new FileRepository<User>(userRepositoryFilepath);
IRepository<Post> postRepository = new FileRepository<Post>(postRepositoryFilepath);
IRepository<Comment> commentRepository = new FileRepository<Comment>(commentRepositoryFilepath);

CliApp cliApp = new CliApp(userRepository, postRepository, commentRepository); 
await cliApp.StartAsync(); 