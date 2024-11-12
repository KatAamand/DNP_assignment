namespace DTO;

public class LoginRequestDTO
{
    public string Username { get; set; }
    public string Password { get; set; }

    public LoginRequestDTO(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public LoginRequestDTO()
    {
    }
}