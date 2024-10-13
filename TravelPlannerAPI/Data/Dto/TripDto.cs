public class TripDto
{
    public record TripDtoInitial(int Id, string Name, string Description, DateTime TripStart, DateTime TripEnd, DateTime CreationDate);
    public record CreateTripDto(string Name, string Description, DateTime TripStart, DateTime TripEnd);

    public record UpdateTripDto(string Name, string Description, DateTime TripStart, DateTime TripEnd);
}