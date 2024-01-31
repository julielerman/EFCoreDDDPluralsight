using AuthorAndBookMaintenance.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace AuthorBook.Infrastructure.Data;

public class AuthorBookContext : DbContext
{
    public AuthorBookContext(DbContextOptions<AuthorBookContext> options) : base(options)
    { }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().OwnsOne(a => a.Name);
    }
}