using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;

        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        // Create a new user
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] CreateUserDTO userRequest)
        {
            var newUser = new User
            {
                Username = userRequest.Username,
                Password = userRequest.Password
            };

            var createdUser = await _userRepository.AddAsync(newUser);

            var userDTO = new UserDTO
            {
                Id = createdUser.Id,
                Username = createdUser.Username
            };

            return Created($"/Users/{userDTO.Id}", userDTO);
        }

        // Get a single user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetSingleUser(int id)
        {
            try
            {
                var user = await _userRepository.GetSingleAsync(id);
                var userDto = new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username
                };
        
                return Ok(userDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        // Get all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers([FromQuery] string? username)
        {
            var users = await _userRepository.GetManyAsync();

            if (!string.IsNullOrEmpty(username))
            {
                users = users.Where(u => u.Username.Contains(username));
            }

            var userDtos = users.Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Username
            }).ToList();

            return Ok(userDtos);
        }

        // Update an existing user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] CreateUserDTO userRequest)
        {
            var user = await _userRepository.GetSingleAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Username = userRequest.Username;
            user.Password = userRequest.Password;

            await _userRepository.UpdateAsync(user);

            return NoContent();
        }

        // Delete a user by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetSingleAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
