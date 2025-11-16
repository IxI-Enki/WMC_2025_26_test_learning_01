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
    /// Validiert interne Loan-Eigenschaften (strukturelle Validierung).
    /// Sammelt alle Validierungsergebnisse und wirft DomainValidationException bei Fehlern.
    /// </summary>
    public static void ValidateLoanInternal( int bookId, string borrowerName, DateTime loanDate )
    {
        var validationResults = new List<DomainValidationResult>
        {
            CheckBookId(bookId),
            CheckBorrowerName(borrowerName),
            CheckLoanDate(loanDate)
        };
        
        foreach (var result in validationResults)
        {
            if (!result.IsValid)
            {
                throw new DomainValidationException(result.Property, result.ErrorMessage!);
            }
        }
    }
}

