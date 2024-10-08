public class Destination
{
    public int Id { get; set; }
    public required string StartLocation { get; set; }
    public required string EndLocation { get; set; }
    public required DateTime CreationDate { get; set; }

    public required Trip Trip { get;set; }
}