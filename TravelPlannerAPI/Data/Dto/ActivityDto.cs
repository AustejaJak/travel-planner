public class ActivityDto
{
    public record ActivityDtoInitial(int Id, string Name, string Type, TimeSpan StartTime, TimeSpan EndTime, DateTime CreationDate);
    public record CreateActivityDto(string Name, string Type, TimeSpan StartTime, TimeSpan EndTime);

    public record UpdateActivityDto(string Name, string Type, TimeSpan StartTime, TimeSpan EndTime);
}