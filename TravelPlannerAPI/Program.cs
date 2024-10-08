using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using O9d.AspNet.FluentValidation;
using static Trip;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TravelDbContext>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


// api/v1/trips GET List 200
// api/v1/trips/{id} GET One 200
// api/v1/trips POST Create 201
// api/v1/trips/{id} PUT/PATCH Modify 200
// api/v1/trips/{id} DELETE Remove 200/204

var tripsGroup = app.MapGroup("/api").WithValidationFilter();

tripsGroup.MapGet("trips", async (TravelDbContext dbContext, CancellationToken cancellationToken) =>
{
    return (await dbContext.Trips.ToListAsync(cancellationToken))
        .Select(trip => new TripDto(trip.Id, trip.Name, trip.Description, trip.TripStart, trip.TripEnd, trip.CreationDate));
});

tripsGroup.MapGet("trips/{tripId}", async (int tripId, TravelDbContext dbContext) =>
{
    var trip = await dbContext.Trips.FirstOrDefaultAsync(t => t.Id == tripId);

    if (trip == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new TripDto(trip.Id, trip.Name, trip.Description, trip.TripStart, trip.TripEnd, trip.CreationDate));
});

tripsGroup.MapPost("trips", async ([Validate] CreateTripDto createTripDto, TravelDbContext dbContext) =>
{
    var trip = new Trip()
    {
        Name = createTripDto.Name,
        Description = createTripDto.Description,
        TripStart = createTripDto.TripStart,
        TripEnd = createTripDto.TripEnd,
        CreationDate = DateTime.UtcNow
    };

    dbContext.Trips.Add(trip);

    await dbContext.SaveChangesAsync();

    return Results.Created($"/api/trips/{trip.Id}", new TripDto(trip.Id, trip.Name, trip.Description, trip.TripStart, trip.TripEnd, trip.CreationDate));
});

tripsGroup.MapPut("trips/{tripId}", async (int tripId, [Validate] UpdateTripDto updateTripDto, TravelDbContext dbContext) =>
{
    var trip = await dbContext.Trips.FirstOrDefaultAsync(t => t.Id == tripId);

    if (trip == null)
    {
        return Results.NotFound();
    }

    trip.Name = updateTripDto.Name;
    trip.Description = updateTripDto.Description;
    trip.TripStart = updateTripDto.TripStart;
    trip.TripEnd = updateTripDto.TripEnd;

    dbContext.Update(trip);

    await dbContext.SaveChangesAsync();

    return Results.Ok(new TripDto(trip.Id, trip.Name, trip.Description, trip.TripStart, trip.TripEnd, trip.CreationDate));
});

tripsGroup.MapDelete("trips/{tripId}", async (int tripId, TravelDbContext dbContext) =>
{
    var trip = await dbContext.Trips.FirstOrDefaultAsync(t => t.Id == tripId);

    if (trip == null)
    {
        return Results.NotFound();
    }

    dbContext.Remove(trip);

    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

var destinationGroup = app.MapGroup("/api/trip/{tripId}").WithValidationFilter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
