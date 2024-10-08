public class Activity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required DateTime Duration { get; set; }
    public required DateTime CreationDate { get; set; }

    public required Destination Destination { get; set; }
}