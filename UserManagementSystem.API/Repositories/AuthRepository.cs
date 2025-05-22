using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserManagementSystem.API.Data;
using UserManagementSystem.API.Interfaces;
using UserManagementSystem.API.Models;
using UserManagementSystem.API.Services;

namespace UserManagementSystem.API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IUserRepository _userRepository;

        public AuthRepository(ApplicationDbContext context, JwtService jwtService, IUserRepository userRepository)
        {
            _context = context;
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var passwordHash = _jwtService.HashPassword(password);
            return await _context.Users.FirstOrDefaultAsync(u =>
                u.Username == username && u.Password == passwordHash);
        }
    }
}
