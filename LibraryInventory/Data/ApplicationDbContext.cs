using LibraryInventory.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryInventory.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author_Book>()
                .HasKey(author_book => new { author_book.AuthorId, author_book.BookISBN });

            modelBuilder.Entity<Author_Book>()
                .HasOne(author_book => author_book.Author)
                .WithMany(author => author.Books)
                .HasForeignKey(author_book => author_book.AuthorId);

            modelBuilder.Entity<Author_Book>()
                .HasOne(author_book => author_book.Book)
                .WithMany(book => book.Authors)
                .HasForeignKey(author_book => author_book.BookISBN);

            modelBuilder.Entity<Book>()
                .HasOne(book => book.Publisher)
                .WithMany(publisher => publisher.Books)
                .HasForeignKey(book => book.PublisherId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Author>? Author { get; set; }
        public DbSet<Author_Book>? Author_Book { get; set; }
        public DbSet<Book>? Book { get; set; }
        public DbSet<Publisher>? Publisher { get; set; }
    }
}