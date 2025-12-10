using Microsoft.EntityFrameworkCore;
using Universal.UsersService.Api.Domain.Entities;

namespace Universal.UsersService.Api.Infrastructure.Persistence
{
    /// <summary>
    /// DbContext para la base de datos InMemory.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
    }
}
