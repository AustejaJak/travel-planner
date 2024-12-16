using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Auth.Model;
using TravelPlannerAPI.Data.Entities;

public class TravelDbContext : IdentityDbContext<TravelMember>
{
    private readonly IConfiguration _configuration;
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Destination> Destinations { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Session> Sessions { get; set; }

    public TravelDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(
            _configuration.GetConnectionString("MySQL"),
            new MySqlServerVersion(new Version(9, 0, 1)),
            options => options.EnableRetryOnFailure(
                maxRetryCount: 5, 
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            )
        );
    }

}