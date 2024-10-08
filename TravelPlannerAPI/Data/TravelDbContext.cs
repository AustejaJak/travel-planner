using Microsoft.EntityFrameworkCore;

public class TravelDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Destination> Destinations { get; set; }
    public DbSet<Activity> Activities { get; set; }

    public TravelDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_configuration.GetConnectionString("MySQL"),
        new MySqlServerVersion(new Version(9, 0, 1)));
    }
}