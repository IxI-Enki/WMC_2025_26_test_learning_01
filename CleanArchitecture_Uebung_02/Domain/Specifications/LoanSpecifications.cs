using Domain.Common;

namespace Domain.Specifications;

public static class LoanSpecifications
{
    public const int BorrowerNameMinLength = 2;
    public const int StandardLoanDurationDays = 14;

    /// <summary>
    /// CheckBookId Validierung.
    /// 
    /// Regel: BookId muss größer als 0 sein
    /// Fehlermeldung: "BookId muss größer als 0 sein."
    /// </summary>
    public static DomainValidationResult CheckBookId( int bookId )
    {
        //throw new NotImplementedException("CheckBookId muss noch implementiert werden!");
        return
            bookId > 0
            ? DomainValidationResult.Success( nameof( bookId ) )
            : DomainValidationResult.Failure( nameof( bookId ), "BookId muss größer als 0 sein." );
    }

    /// <summary>
    /// CheckBorrowerName Validierung.
    /// 
    /// Regeln:
    /// - Darf nicht leer sein
    /// - Muss mindestens BorrowerNameMinLength Zeichen haben
    /// 
    /// Fehlermeldungen:
    /// - "BorrowerName darf nicht leer sein."
    /// - "BorrowerName muss mindestens {BorrowerNameMinLength} Zeichen haben."
    /// </summary>
    public static DomainValidationResult CheckBorrowerName( string borrowerName )
    {
        //throw new NotImplementedException("CheckBorrowerName muss noch implementiert werden!");
        return
            string.IsNullOrWhiteSpace( borrowerName )
            ? DomainValidationResult.Failure( nameof( borrowerName ), "BorrowerName darf nicht leer sein." )
            : borrowerName.Length < BorrowerNameMinLength
                ? DomainValidationResult.Failure( nameof( borrowerName ), $"BorrowerName muss mindestens {BorrowerNameMinLength} Zeichen haben." )
                : DomainValidationResult.Success( nameof( borrowerName ) );
    }

    /// <summary>
    /// CheckLoanDate Validierung.
    /// 
    /// Regel: LoanDate darf nicht in der Zukunft liegen
    /// Fehlermeldung: "LoanDate darf nicht in der Zukunft liegen."
    /// </summary>
    public static DomainValidationResult CheckLoanDate( DateTime loanDate )
    {
        //throw new NotImplementedException( "CheckLoanDate muss noch implementiert werden!" );
        return
            loanDate <= DateTime.Now
            ? DomainValidationResult.Success( nameof( loanDate ) )
            : DomainValidationResult.Failure( nameof( loanDate ), "LoanDate darf nicht in der Zukunft liegen." );
    }

    /// <summary>
    /// TODO: Implementiere die ValidateLoanInternal Methode.
    /// 
    /// Diese Methode soll:
    /// 1. Eine Liste von DomainValidationResult erstellen
    /// 2. CheckBookId, CheckBorrowerName und CheckLoanDate aufrufen
    /// 3. Über alle Ergebnisse iterieren
    /// 4. Bei Fehler eine DomainValidationException werfen
    /// </summary>
    public static void ValidateLoanInternal( int bookId, string borrowerName, DateTime loanDate )
    {
        return;
        throw new NotImplementedException( "ValidateLoanInternal muss noch implementiert werden!" );
    }
}

