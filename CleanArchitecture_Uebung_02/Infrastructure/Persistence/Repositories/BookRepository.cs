using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// TODO: Implementiere die spezifischen Repository-Methoden für Book.
/// </summary>
public class BookRepository(AppDbContext ctx) : GenericRepository<Book>(ctx), IBookRepository
{
    /// <summary>
    /// TODO: Implementiere GetByISBNAsync - Buch per ISBN laden (mit Include Author).
    /// </summary>
    public async Task<Book?> GetByISBNAsync(string isbn, CancellationToken ct = default)
    {
        throw new NotImplementedException("BookRepository.GetByISBNAsync muss noch implementiert werden!");
    }

    /// <summary>
    /// TODO: Implementiere GetBooksByAuthorAsync - Alle Bücher eines Autors.
    /// </summary>
    public async Task<IReadOnlyCollection<Book>> GetBooksByAuthorAsync(int authorId, CancellationToken ct = default)
    {
        throw new NotImplementedException("BookRepository.GetBooksByAuthorAsync muss noch implementiert werden!");
    }

    public Task<IReadOnlyCollection<Book>> GetBooksByhAuthorAsync( int authorId, CancellationToken ct = default )
    {
        throw new NotImplementedException( );
    }
}
