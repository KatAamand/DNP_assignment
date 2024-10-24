using Entities;
using FileIORepositories;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepository<Post>>(provider => new FileRepository<Post>("posts.json"));
builder.Services.AddScoped<IRepository<User>>(provider => new FileRepository<User>("users.json"));
builder.Services.AddScoped<IRepository<Comment>>(provider => new FileRepository<Comment>("comments.json"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();