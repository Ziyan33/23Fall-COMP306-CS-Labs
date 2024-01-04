using BookResearcher.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookResearcher.Domain.Data
{
    public class BookResearcherContext : DbContext
    {
        public BookResearcherContext(DbContextOptions<BookResearcherContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<LibraryBranch> LibraryBranches { get; set; }
        public DbSet<BookAuthors> BookAuthors { get; set; }
        public DbSet<BookAvailability> BookAvailability { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>()
           .Property(b => b.AuthorID)
           .ValueGeneratedOnAdd(); // Confirm this is set correctly

            modelBuilder.Entity<Book>()
            .Property(b => b.BookID)
            .ValueGeneratedOnAdd(); // Confirm this is set correctly

            modelBuilder.Entity<LibraryBranch>()
            .Property(lb => lb.LibraryID)
            .ValueGeneratedOnAdd(); // Confirm this is set correctly

            // Composite primary key for BookAuthors
            modelBuilder.Entity<BookAuthors>()
                .HasKey(ba => new { ba.BookID, ba.AuthorID });

            modelBuilder.Entity<BookAuthors>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookID);

            modelBuilder.Entity<BookAuthors>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorID);

            // Composite primary key for BookAvailability
            modelBuilder.Entity<BookAvailability>()
                .HasKey(ba => new { ba.BookID, ba.LibraryID });

            modelBuilder.Entity<BookAvailability>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAvailability)
                .HasForeignKey(ba => ba.BookID);

            modelBuilder.Entity<BookAvailability>()
                .HasOne(ba => ba.LibraryBranch)
                .WithMany(lb => lb.BookAvailability)
                .HasForeignKey(ba => ba.LibraryID);

            // Add any additional model configuration here
        }
    }
}
