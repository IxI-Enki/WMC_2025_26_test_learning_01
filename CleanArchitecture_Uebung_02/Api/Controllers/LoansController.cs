using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Endpunkte für Ausleihen.
/// 
/// TODO: Implementiere die folgenden Endpoints:
/// - POST /api/loans - CreateLoan (Buch ausleihen)
/// - PUT /api/loans/{id}/return - ReturnLoan (Buch zurückgeben)
/// - GET /api/loans/book/{bookId} - GetLoansByBook
/// - GET /api/loans/overdue - GetOverdueLoans
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LoansController(IMediator mediator) : ControllerBase
{
}

