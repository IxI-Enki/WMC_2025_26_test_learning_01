using Application.Features.Authors.Queries.GetAuthorById;
using FluentValidation;

namespace Application.Features.Books.Queries.GetBookById;

public class
    GetBoookyIdQueryValidator : AbstractValidator<GetAuthorByIdQuery>
{
    public GetBoookyIdQueryValidator( )
    {
        RuleFor( x => x.Id )
            .GreaterThan( 0 )
            .WithMessage( "Id must be greater than 0." );
    }
}
