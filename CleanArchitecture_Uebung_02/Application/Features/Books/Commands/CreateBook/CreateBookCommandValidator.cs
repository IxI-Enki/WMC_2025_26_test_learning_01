using FluentValidation;

namespace Application.Features.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.ISBN)
            .NotEmpty()
            .Length(13).WithMessage("ISBN muss genau 13 Zeichen haben.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(1);

        RuleFor(x => x.AuthorId)
            .GreaterThan(0);

        RuleFor(x => x.PublicationYear)
            .GreaterThanOrEqualTo(1450)
            .LessThanOrEqualTo(DateTime.Now.Year + 1);

        RuleFor(x => x.AvailableCopies)
            .GreaterThanOrEqualTo(0);
    }
}

