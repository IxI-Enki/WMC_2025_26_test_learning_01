using FluentValidation;

namespace Application.Features.Authors.Queries.GetAuthorById;

public class
    GetAuthorByIdQueryValidator : AbstractValidator<GetAuthorByIdQuery>
{
    public GetAuthorByIdQueryValidator( )
    {
        RuleFor(x => x.Id)
            .GreaterThan( 0 )
            .WithMessage( "Id must be greater than 0." );
    }
}
