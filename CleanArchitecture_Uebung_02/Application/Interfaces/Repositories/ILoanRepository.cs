using Domain.Entities;

namespace Application.Interfaces.Repositories;

/// <summary>
/// Loan-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public interface ILoanRepository : IGenericRepository<Loan>
{
    Task<IReadOnlyCollection<Loan>> GetLoansByBookIdAsync(int bookId, CancellationToken ct = default);
    Task<IReadOnlyCollection<Loan>> GetActiveLoansByBorrowerAsync(string borrowerName, CancellationToken ct = default);
    Task<IReadOnlyCollection<Loan>> GetOverdueLoansAsync(CancellationToken ct = default);
}

