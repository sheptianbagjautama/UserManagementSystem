using Microsoft.EntityFrameworkCore;
using UserManagementSystem.API.Models;

namespace UserManagementSystem.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();
    }
}
