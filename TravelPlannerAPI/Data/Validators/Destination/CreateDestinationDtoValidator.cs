using FluentValidation;
using static DestinationDto;
public class CreateDestinationDtoValidator : AbstractValidator<CreateDestinationDto>
{
    public CreateDestinationDtoValidator()
    {
        RuleFor(dto => dto.StartLocation).NotEmpty().NotNull().Length(min: 2, max: 100);
        RuleFor(dto => dto.EndLocation).NotEmpty().NotNull().Length(min: 5, max: 500);
    }
}