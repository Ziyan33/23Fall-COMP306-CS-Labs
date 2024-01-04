using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using _301133339_liu_lab3.Models;
using Microsoft.AspNetCore.Identity;

namespace _301133339_liu_lab3.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Your existing DbSets...
        //public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This should be called first

            // Your existing model configurations...
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.User)
                .WithMany(u => u.Movies)
                .HasForeignKey(m => m.UserId);
            modelBuilder.Entity<Comment>().HasNoKey();
            // ... Other model configurations
        }

        // ... Other DbSet properties
    }
}
