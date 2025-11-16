namespace Application.Dtos;

/// <summary>
/// DTO für die Rückgabe von Loan-Informationen an die API.
/// IsOverdue wird zur Laufzeit berechnet (ReturnDate == null && DateTime.Now > DueDate).
/// </summary>
public sealed record GetLoanDto(
    int Id,
    int BookId,
    string BookTitle,
    string BorrowerName,
    DateTime LoanDate,
    DateTime DueDate,
    DateTime? ReturnDate,
    bool IsOverdue
);

