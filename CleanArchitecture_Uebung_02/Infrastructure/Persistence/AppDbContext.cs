using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext. Verwaltet die Verbindung zur Datenbank und das Mapping der Entitäten.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(book =>
        {
            book.Property(b => b.ISBN).HasMaxLength(13).IsRequired();
            book.Property(b => b.Title).HasMaxLength(500).IsRequired();
            book.Property(b => b.PublicationYear).IsRequired();
            book.Property(b => b.AvailableCopies).IsRequired();
            book.Property(b => b.RowVersion).IsRowVersion();
            
            // Unique Index auf ISBN
            book.HasIndex(b => b.ISBN).IsUnique().HasDatabaseName("UX_Books_ISBN");
            
            // Beziehung zu Author
            book.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Author>(author =>
        {
            author.Property(a => a.FirstName).HasMaxLength(100).IsRequired();
            author.Property(a => a.LastName).HasMaxLength(100).IsRequired();
            author.Property(a => a.DateOfBirth).IsRequired();
            author.Property(a => a.RowVersion).IsRowVersion();
        });

        modelBuilder.Entity<Loan>(loan =>
        {
            loan.Property(l => l.BorrowerName).HasMaxLength(200).IsRequired();
            loan.Property(l => l.LoanDate).IsRequired();
            loan.Property(l => l.DueDate).IsRequired();
            loan.Property(l => l.RowVersion).IsRowVersion();
            
            // Index für häufige Abfragen
            loan.HasIndex(l => new { l.BookId, l.ReturnDate });
            
            // Beziehung zu Book
            loan.HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

