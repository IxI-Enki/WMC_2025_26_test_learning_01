using FluentValidation; 

namespace Application.Features.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator( )
    {
        RuleFor( x => x.ISBN )
            .NotEmpty( ).WithMessage( "ISBN darf nicht leer sein." )
            .Length( 13 ).WithMessage( "ISBN muss genau 13 Zeichen haben." )
            .Matches( "^[0-9]+$" ).WithMessage( "ISBN darf nur Ziffern enthalten." );

        RuleFor( x => x.Title )
            .NotEmpty( ).WithMessage( "Titel darf nicht leer sein." )
            .MinimumLength( 1 );

        RuleFor( x => x.PublicationYear )
            .GreaterThanOrEqualTo( 1450 )
            .LessThanOrEqualTo( DateTime.Now.Year + 1 )
            .WithMessage( $"PublicationYear muss zwischen 1450 und {DateTime.Now.Year + 1} liegen." );

        RuleFor( x => x.AvailableCopies )
            .GreaterThanOrEqualTo( 0 ).WithMessage( "AvailableCopies muss mindestens 0 sein." );
    }
}