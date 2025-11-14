using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// TODO: Implementiere die spezifischen Methoden für Loan Repository.
/// 
/// Die folgenden Methoden müssen implementiert werden:
/// - GetLoansByBookIdAsync: Alle Ausleihen für ein Buch (Include Book)
/// - GetActiveLoansByBorrowerAsync: Aktive Ausleihen (ReturnDate == null) für einen Ausleiher
/// - GetOverdueLoansAsync: Alle überfälligen Ausleihen (ReturnDate == null && DueDate < DateTime.Now)
/// </summary>
public class LoanRepository(AppDbContext ctx) : GenericRepository<Loan>(ctx), ILoanRepository
{
    public async Task<IReadOnlyCollection<Loan>> GetLoansByBookIdAsync(int bookId, CancellationToken ct = default)
    {
        // TODO: Implementiere diese Methode
        throw new NotImplementedException("GetLoansByBookIdAsync muss noch implementiert werden!");
    }

    public async Task<IReadOnlyCollection<Loan>> GetActiveLoansByBorrowerAsync(string borrowerName, CancellationToken ct = default)
    {
        // TODO: Implementiere diese Methode
        throw new NotImplementedException("GetActiveLoansByBorrowerAsync muss noch implementiert werden!");
    }

    public async Task<IReadOnlyCollection<Loan>> GetOverdueLoansAsync(CancellationToken ct = default)
    {
        // TODO: Implementiere diese Methode
        throw new NotImplementedException("GetOverdueLoansAsync muss noch implementiert werden!");
    }
}

