using UserManagementSystem.API.Dtos;
using UserManagementSystem.API.Models;
using UserManagementSystem.API.Services;

namespace UserManagementSystem.API.Mappings
{
    public static class UserMapping
    {
        public static ResponseUserDto ToDto(this User user)
        {
            return new ResponseUserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName
            };
        }

        public static User ToModel(this RequestUserDto dto, string hashedPassword)
        {
            return new User
            {
                Username = dto.Username,
                FullName = dto.FullName,
                Password = hashedPassword
            };
        }
    }
}
