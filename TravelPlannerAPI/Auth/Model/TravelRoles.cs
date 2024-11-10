namespace TravelPlannerAPI.Auth.Model;

public class TravelRoles
{
    public const string Admin = nameof(Admin);
    public const string TravelMember = nameof(TravelMember);

    public static readonly IReadOnlyCollection<string> All = new[] { Admin, TravelMember };
}