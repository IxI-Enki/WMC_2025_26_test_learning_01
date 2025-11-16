using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Loan-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public class LoanRepository(AppDbContext ctx) : GenericRepository<Loan>(ctx), ILoanRepository
{
    /// <summary>
    /// Alle Ausleihen für ein bestimmtes Buch (mit Book-Navigation Property).
    /// </summary>
    public async Task<IReadOnlyCollection<Loan>> GetLoansByBookIdAsync(int bookId, CancellationToken ct = default)
    {
        return await Set
            .AsNoTracking()
            .Include(l => l.Book)
            .Where(l => l.BookId == bookId)
            .OrderByDescending(l => l.LoanDate)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Alle aktiven Ausleihen (noch nicht zurückgegeben) für einen bestimmten Ausleiher.
    /// </summary>
    public async Task<IReadOnlyCollection<Loan>> GetActiveLoansByBorrowerAsync(string borrowerName, CancellationToken ct = default)
    {
        return await Set
            .AsNoTracking()
            .Include(l => l.Book)
            .Where(l => l.BorrowerName == borrowerName && l.ReturnDate == null)
            .OrderBy(l => l.DueDate)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Alle überfälligen Ausleihen (nicht zurückgegeben und Frist abgelaufen).
    /// </summary>
    public async Task<IReadOnlyCollection<Loan>> GetOverdueLoansAsync(CancellationToken ct = default)
    {
        var now = DateTime.Now;
        return await Set
            .AsNoTracking()
            .Include(l => l.Book)
            .Where(l => l.ReturnDate == null && l.DueDate < now)
            .OrderBy(l => l.DueDate)
            .ToListAsync(ct);
    }
}

