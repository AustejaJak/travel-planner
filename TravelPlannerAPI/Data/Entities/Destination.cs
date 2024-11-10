using System.ComponentModel.DataAnnotations;
using TravelPlannerAPI.Auth.Model;

public class Destination
{
    public int Id { get; set; }
    public required string StartLocation { get; set; }
    public required string EndLocation { get; set; }
    public required DateTime CreationDate { get; set; }

    public required Trip Trip { get; set; }
    
    [Required]
    public required string UserId { get; set; }
    public TravelMember User { get; set; }
}