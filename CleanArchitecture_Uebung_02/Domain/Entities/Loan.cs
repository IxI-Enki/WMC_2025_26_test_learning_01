using Domain.Specifications;

namespace Domain.Entities;

/// <summary>
/// Repräsentiert eine Buchausleihe.
/// </summary>
public class Loan : BaseEntity
{
    /// <summary>
    /// Fremdschlüssel auf das Buch.
    /// </summary>
    public int BookId { get; private set; }
    
    /// <summary>
    /// Navigation zum Buch.
    /// </summary>
    public Book Book { get; private set; } = null!;
    
    /// <summary>
    /// Name des Ausleihers.
    /// </summary>
    public string BorrowerName { get; private set; } = string.Empty;
    
    /// <summary>
    /// Ausleihdatum.
    /// </summary>
    public DateTime LoanDate { get; private set; }
    
    /// <summary>
    /// Geplantes Rückgabedatum.
    /// </summary>
    public DateTime DueDate { get; private set; }
    
    /// <summary>
    /// Tatsächliches Rückgabedatum (null = noch nicht zurückgegeben).
    /// </summary>
    public DateTime? ReturnDate { get; private set; }
    
    private Loan() { } // Für EF Core

    /// <summary>
    /// Erstellt eine neue Loan-Entität (Factory-Methode).
    /// Führt Domänenvalidierungen durch und wirft DomainValidationException bei Fehlern.
    /// </summary>
    public static Loan Create(Book book, string borrowerName, DateTime loanDate)
    {
        ArgumentNullException.ThrowIfNull(book);
        
        var trimmedBorrowerName = (borrowerName ?? string.Empty).Trim();
        
        // Domänenvalidierungen (BorrowerName und LoanDate)
        // BookId validation is skipped here since we have the Book object directly
        var borrowerNameValidation = LoanSpecifications.CheckBorrowerName(trimmedBorrowerName);
        if (!borrowerNameValidation.IsValid)
            throw new Exceptions.DomainValidationException(borrowerNameValidation.Property, borrowerNameValidation.ErrorMessage!);
        
        var loanDateValidation = LoanSpecifications.CheckLoanDate(loanDate);
        if (!loanDateValidation.IsValid)
            throw new Exceptions.DomainValidationException(loanDateValidation.Property, loanDateValidation.ErrorMessage!);
        
        return new Loan
        {
            Book = book,
            BookId = book.Id,
            BorrowerName = trimmedBorrowerName,
            LoanDate = loanDate,
            DueDate = loanDate.AddDays(LoanSpecifications.StandardLoanDurationDays),
            ReturnDate = null
        };
    }

    /// <summary>
    /// Markiert das Buch als zurückgegeben.
    /// </summary>
    public void MarkAsReturned(DateTime returnDate)
    {
        ReturnDate = returnDate;
    }

    /// <summary>
    /// Prüft, ob die Ausleihe überfällig ist.
    /// </summary>
    public bool IsOverdue() => ReturnDate == null && DateTime.Now > DueDate;
}

