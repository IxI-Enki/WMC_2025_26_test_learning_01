using Domain.Entities;

namespace Application.Interfaces.Repositories;

/// <summary>
/// Loan-spezifische Repository-Methoden zusätzlich zu den generischen CRUD-Operationen.
/// </summary>
public interface ILoanRepository : IGenericRepository<Loan>
{
    /// <summary>
    /// Alle Ausleihen für ein bestimmtes Buch.
    /// </summary>
    Task<IReadOnlyCollection<Loan>> GetLoansByBookIdAsync(int bookId, CancellationToken ct = default);
    
    /// <summary>
    /// Alle aktiven Ausleihen (noch nicht zurückgegeben) für einen bestimmten Ausleiher.
    /// </summary>
    Task<IReadOnlyCollection<Loan>> GetActiveLoansByBorrowerAsync(string borrowerName, CancellationToken ct = default);
    
    /// <summary>
    /// Alle überfälligen Ausleihen (nicht zurückgegeben und Frist abgelaufen).
    /// </summary>
    Task<IReadOnlyCollection<Loan>> GetOverdueLoansAsync(CancellationToken ct = default);
}
