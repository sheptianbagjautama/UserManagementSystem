using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserManagementSystem.API.Data;
using UserManagementSystem.API.Interfaces;
using UserManagementSystem.API.Models;

namespace UserManagementSystem.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var passwordHash = HashPassword(password);
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == passwordHash);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> CreateAsync(User user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {   
                var existingUser = await GetByUsernameAsync(user.Username);
                if(existingUser != null)
                {
                    throw new Exception("Username already exists.");
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return user;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<User?> UpdateAsync(int id, User user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existing = await _context.Users.FindAsync(id);
                if (existing == null) return null;

                if(user.Username != existing.Username) {
                    var findUser = await GetByUsernameAsync(user.Username);
                    if (findUser != null)
                    {
                        throw new Exception("Username already exists.");
                    }
                }

                existing.Username = user.Username;
                existing.FullName = user.FullName;
                if (!string.IsNullOrWhiteSpace(user.Password))
                    existing.Password = user.Password;

                existing.Gender = user.Gender;
                existing.BirthDate = user.BirthDate;
                existing.Address = user.Address;
                existing.Phone = user.Phone;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return existing;
            } catch(Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message.ToString()); 
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return false;

                _context.Users.Remove(user);
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            } catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message.ToString());
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var byts = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(byts);
            return Convert.ToBase64String(hash);
        }
    }
}
