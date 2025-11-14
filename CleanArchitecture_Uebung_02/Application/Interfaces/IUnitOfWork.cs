using Application.Interfaces.Repositories;

namespace Application.Interfaces;

/// <summary>
/// Aggregiert Repositories und speichert Änderungen. Sicherer Umgang mit Transaktionen.
/// </summary>
public interface IUnitOfWork
{
    IBookRepository Books { get; }
    IAuthorRepository Authors { get; }
    ILoanRepository Loans { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

