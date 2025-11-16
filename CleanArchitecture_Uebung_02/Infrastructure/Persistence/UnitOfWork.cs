using Application.Interfaces;
using Application.Interfaces.Repositories;

namespace Infrastructure.Persistence;

public class UnitOfWork(
    AppDbContext dbContext,
    IBookRepository books,
    IAuthorRepository authors,
    ILoanRepository loans )
        : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _dbContext = dbContext;

    private bool _disposed;

    public IBookRepository Books => books;
    public IAuthorRepository Authors => authors;
    public ILoanRepository Loans => loans;

    public Task<int> SaveChangesAsync( CancellationToken ct = default ) => _dbContext.SaveChangesAsync( ct );

    public void Dispose( )
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    protected virtual void Dispose( bool disposing )
    {
        if(_disposed) return;
        if(disposing) _dbContext.Dispose( );
        _disposed = true;
    }
}
