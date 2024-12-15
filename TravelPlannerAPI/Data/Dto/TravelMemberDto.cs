namespace TravelPlannerAPI.Data.Dto;

public class TravelMemberDto
{
    public record TravelMemberDtoInitial(string id, string userName, string email, string role);
}