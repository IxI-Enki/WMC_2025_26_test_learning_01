using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Books.Commands.UpdateBook
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator( )
        {
            RuleFor( x => x.Id )
                .GreaterThan( 0 );

            RuleFor( x => x.AuthorId )
                .NotEmpty( );

            RuleFor( x => x.Title )
                .NotEmpty( )
                .MinimumLength( 2 )
                .WithMessage( "Title must be at least 2 chars long." );
        }
    }
}
