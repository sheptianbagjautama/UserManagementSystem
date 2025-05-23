using UserManagementSystem.API.Dtos;
using UserManagementSystem.API.Models;

public static class UserMapping
{
    public static ResponseUserDto ToDto(this User user)
    {
        return new ResponseUserDto
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Gender = user.Gender,
            BirthDate = user.BirthDate,
            Address = user.Address,
            Phone = user.Phone
        };
    }

    public static User ToModel(this RequestUserDto dto, string hashedPassword)
    {
        return new User
        {
            Username = dto.Username,
            FullName = dto.FullName,
            Password = hashedPassword,
            Gender = dto.Gender,
            BirthDate = dto.BirthDate,
            Address = dto.Address,
            Phone = dto.Phone
        };
    }
}
