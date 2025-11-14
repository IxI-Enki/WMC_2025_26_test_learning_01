using Domain.Entities;

namespace Application.Interfaces.Repositories;

/// <summary>
/// Loan-spezifische Abfragen zusätzlich zu den generischen CRUDs.
/// </summary>
public interface ILoanRepository : IGenericRepository<Loan>
{
    // TODO: Implementiere die spezifischen Repository-Methoden für Loan.
}
