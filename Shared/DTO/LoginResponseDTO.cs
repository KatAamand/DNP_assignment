namespace DTO;

public class LoginResponseDTO
{
    public bool IsAuthenticated { get; set; }
    public UserDTO UserDto { get; set; }
}