using ContactApp.Model;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=root;");
        }
    }
}