using Microsoft.EntityFrameworkCore;
using MyAPI.Models;

namespace MyAPI.Context
{
    public class InMemoryContext : DbContext
    {

        public DbSet<Song> Song { get; set; }

        public DbSet<User> User { get; set; }

        public InMemoryContext(DbContextOptions<InMemoryContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
