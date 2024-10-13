using FluentValidation;
using static TripDto;
public class CreateTripDtoValidator : AbstractValidator<CreateTripDto>
{
    public CreateTripDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().NotNull().Length(min: 2, max: 100);
        RuleFor(dto => dto.Description).NotEmpty().NotNull().Length(min: 5, max: 500);
        RuleFor(dto => dto.TripStart).NotEmpty().NotNull().Must(BeAValidDate).GreaterThan(d => DateTime.Now);
        RuleFor(dto => dto.TripEnd).NotEmpty().NotNull().Must(BeAValidDate).GreaterThan(dto => dto.TripStart).GreaterThan(d => DateTime.Now);
    }

    private bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default);
    }
}