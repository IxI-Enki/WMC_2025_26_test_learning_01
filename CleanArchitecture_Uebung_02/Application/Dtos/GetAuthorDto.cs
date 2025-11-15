using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public sealed record GetAuthorDto(
        int Id,
        string FirstName,
        string LastName,
        DateTime DateOfBirth
        //ICollection<GetBookDto> Books
        );
}
