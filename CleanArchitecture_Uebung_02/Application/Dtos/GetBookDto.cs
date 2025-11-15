using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public sealed record GetBookDto(
        int Id,
        string ISBN,
        string Title,
        int AuhorId,
        string AuthorName,
        int PublicationYear,
        int AvaliableCopies 
        );
}
