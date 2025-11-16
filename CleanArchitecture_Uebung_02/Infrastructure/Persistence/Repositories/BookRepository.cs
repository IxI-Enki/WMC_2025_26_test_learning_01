using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// TODO: Implementiere die spezifischen Repository-Methoden für Book.
/// </summary>
public class BookRepository( AppDbContext ctx ) : GenericRepository<Book>( ctx ), IBookRepository
{
    /// <summary>
    /// GetByISBNAsync - Buch per ISBN laden (mit Include Author).
    /// </summary>
    public async Task<Book?> GetByISBNAsync( string isbn, CancellationToken ct = default )
    {
        var result = await Set.FirstOrDefaultAsync(s => s.ISBN == isbn, ct) ;
        //if(result is not null)
        //{
        //    result.Author = await Set ;
        //}

        return result;
    }

    /// <summary>
    ///  GetByBookIdAsync -  Buch per Id laden (mit Include Author).
    /// </summary>
    /// <param name="bookId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<Book?> GetByBookIdAsync( int bookId, CancellationToken ct = default )
    {
        var result = await Set.FirstOrDefaultAsync(s => s.Id == bookId,ct);



        return result;
    }

    /// <summary>
    /// TODO: Implementiere GetBooksByAuthorAsync - Alle Bücher eines Autors.
    /// </summary>
    public async Task<IReadOnlyCollection<Book>> GetBooksByAuthorAsync( int authorId, CancellationToken ct = default )
    {
        throw new NotImplementedException( "BookRepository.GetBooksByAuthorAsync muss noch implementiert werden!" );
    }

    public Task<IReadOnlyCollection<Book>> GetBooksByhAuthorAsync( int authorId, CancellationToken ct = default )
    {
        throw new NotImplementedException( );
    }

}
