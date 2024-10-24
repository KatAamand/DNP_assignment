using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
public abstract class ControllerBase : Controller
{
    // Helper method to check if an entity exists by a specific id
    protected async Task<bool> EntityExistsAsync<T>(IRepository<T> repository, int entityId) where T : class, IEntity
    {
        
        return (await repository.GetManyAsync()).Any(e => e.Id == entityId);
    }
}