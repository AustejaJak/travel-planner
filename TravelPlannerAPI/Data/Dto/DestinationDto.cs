public class DestinationDto
{
    public record DestinationDtoInitial(int Id, string StartLocation, string EndLocation, DateTime CreationDate);
    public record CreateDestinationDto(string StartLocation, string EndLocation);

    public record UpdateDestinationDto(string StartLocation, string EndLocation);
}