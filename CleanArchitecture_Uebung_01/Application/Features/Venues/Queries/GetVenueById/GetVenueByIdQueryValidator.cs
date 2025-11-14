using FluentValidation;

namespace Application.Features.Venues.Queries.GetVenueById;

public class GetVenueByIdQueryValidator : AbstractValidator<GetVenueByIdQuery>
{
    public GetVenueByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}
