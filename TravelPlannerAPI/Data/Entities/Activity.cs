using System.ComponentModel.DataAnnotations;
using TravelPlannerAPI.Auth.Model;

public class Activity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required TimeSpan StartTime { get; set; }
     public required TimeSpan EndTime { get; set; }
    public required DateTime CreationDate { get; set; }

    public required Destination Destination { get; set; }
    
    [Required]
    public required string UserId { get; set; }
    public TravelMember User { get; set; }
}