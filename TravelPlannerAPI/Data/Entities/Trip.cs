using FluentValidation;

public class Trip
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required DateTime TripStart { get; set; }
    public required DateTime TripEnd { get; set; }
    public required DateTime CreationDate { get; set; }
    public DateTime? ExpiresIn { get; set; }


    public record TripDto(int Id, string Name, string Description, DateTime TripStart, DateTime TripEnd, DateTime CreationDate);
    public record CreateTripDto(string Name, string Description, DateTime TripStart, DateTime TripEnd);

    public record UpdateTripDto(string Name, string Description, DateTime TripStart, DateTime TripEnd);

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

    public class UpdateTripDtoValidator : AbstractValidator<UpdateTripDto>
    {
        public UpdateTripDtoValidator()
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
}