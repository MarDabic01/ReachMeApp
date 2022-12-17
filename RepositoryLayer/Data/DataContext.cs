using DomainLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Follow> Follows { get; set; }
    }
}
