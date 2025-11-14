namespace Application.Dtos;

public record GetLoanDto(int Id, int BookId, string BookTitle, string BorrowerName, 
    DateTime LoanDate, DateTime DueDate, DateTime? ReturnDate, bool IsOverdue);

