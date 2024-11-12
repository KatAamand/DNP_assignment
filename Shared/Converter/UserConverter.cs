using DTO;
using Entities;

namespace Converter;

public class UserConverter
{
    public static User toUser(UserDTO userDto)
    {
        User converterUser = new User();
        converterUser.Id = userDto.Id; 
        converterUser.Username = userDto.Username;
        
        return converterUser;
    }

    public static UserDTO toDto(User user)
    {
        UserDTO converterUserDTO = new UserDTO();
        converterUserDTO.Id = user.Id;
        converterUserDTO.Username = user.Username;
        
        return converterUserDTO;
    }
}