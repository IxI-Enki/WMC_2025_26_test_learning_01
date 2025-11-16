using Application.Dtos;
using Domain.Entities;
using Mapster;

namespace Application.Common.Mappings;

/// <summary>
/// Mapster-Konfiguration für Loan → GetLoanDto.
/// Mapped BookTitle aus Book.Title und berechnet IsOverdue zur Laufzeit.
/// </summary>
public static class LoanMappingConfig
{
    public static void ConfigureLoanMappings()
    {
        TypeAdapterConfig<Loan, GetLoanDto>.NewConfig()
            .Map(dest => dest.BookTitle, src => src.Book.Title)
            .Map(dest => dest.IsOverdue, src => src.IsOverdue());
    }
}
