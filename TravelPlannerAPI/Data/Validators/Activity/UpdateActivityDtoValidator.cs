using FluentValidation;
using static ActivityDto;
public class UpdateActivityDtoValidator : AbstractValidator<UpdateActivityDto>
{
    public UpdateActivityDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().NotNull().Length(min: 2, max: 100);
        RuleFor(dto => dto.Type).NotEmpty().NotNull().Length(min: 2, max: 100);
        RuleFor(activity => activity.StartTime).NotEmpty().WithMessage("Start time is required.").Must(BeAValidTime).WithMessage("Start time must be in a valid format (e.g., HH:mm:ss).");
        RuleFor(activity => activity.EndTime).NotEmpty().WithMessage("End time is required.").Must(BeAValidTime).WithMessage("End time must be in a valid format (e.g., HH:mm:ss).");
        RuleFor(activity => activity.EndTime).GreaterThan(activity => activity.StartTime).WithMessage("End time must be after start time.");
    }
    private bool BeAValidTime(TimeSpan time)
    {
       return time != TimeSpan.Zero;
    }
}