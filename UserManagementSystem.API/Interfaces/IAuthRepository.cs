using UserManagementSystem.API.Models;

namespace UserManagementSystem.API.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> AuthenticateAsync(string username, string password);
    }
}
