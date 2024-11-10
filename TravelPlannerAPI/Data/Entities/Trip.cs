using System.ComponentModel.DataAnnotations;
using FluentValidation;
using TravelPlannerAPI.Auth.Model;

public class Trip
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required DateTime TripStart { get; set; }
    public required DateTime TripEnd { get; set; }
    public required DateTime CreationDate { get; set; }
    public DateTime? ExpiresIn { get; set; }

    [Required]
    public required string UserId { get; set; }
    public TravelMember User { get; set; }
}