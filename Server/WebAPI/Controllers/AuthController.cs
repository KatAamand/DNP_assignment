using Converter;
using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers 
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController : ControllerBase 
    {
        private readonly IRepository<User> _userRepository;

        public AuthController(IRepository<User> userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }
        
        // Login user 
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            try
            {
                if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) ||
                    string.IsNullOrEmpty(loginRequest.Password))
                {
                    return BadRequest("Invalid login request");
                }
                
                var user = (await _userRepository.GetManyAsync())
                    .FirstOrDefault(u => u.Username == loginRequest.Username);
                
                // Validation
                if (user == null || user.Password != loginRequest.Password)
                {
                    return Unauthorized("Invalid username or password.");
                }
                
                var response = new LoginResponseDTO
                {
                    IsAuthenticated = true,
                    UserDto = UserConverter.toDto(user)
                };
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating user: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
           
        }
    }
}