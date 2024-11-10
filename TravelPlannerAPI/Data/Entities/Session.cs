using System.ComponentModel.DataAnnotations;
using TravelPlannerAPI.Auth.Model;

namespace TravelPlannerAPI.Data.Entities;

public class Session
{
    public Guid Id { get; set; }
    public string LastRefreshToken { get; set; }
    public DateTimeOffset InitiateAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }

    [Required] 
    public required string UserId { get; set; }
    public TravelMember User { get; set; }
}