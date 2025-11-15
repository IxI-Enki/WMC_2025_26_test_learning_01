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
    /// TODO: Implementiere die Create-Methode für Loan.
    /// 
    /// Diese Methode soll:
    /// 1. ArgumentNullException.ThrowIfNull(book) aufrufen
    /// 2. borrowerName trimmen
    /// 3. LoanSpecifications.ValidateLoanInternal aufrufen
    /// 4. Ein neues Loan-Objekt erstellen und zurückgeben
    /// 5. DueDate soll LoanDate + 14 Tage sein
    /// </summary>
    public static Loan Create(Book book, string borrowerName, DateTime loanDate)
    {



        throw new NotImplementedException("Loan.Create muss noch implementiert werden!");
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

