using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using O9d.AspNet.FluentValidation;
using TravelPlannerAPI.Auth.Model;
using TravelPlannerAPI.Helpers;
using static TripDto;

[ApiController]
[Route("api/v1/[controller]")]
public class TripsController : ControllerBase
{

    private readonly TravelDbContext _dbContext;
    public TripsController(TravelDbContext dbContext)
    {

        _dbContext = dbContext;
    }

    // GET: api/v1/trips
    // /posts?pageNumber=1&pageSize=5
    [HttpGet(Name = "GetTrips")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTrips([FromQuery] SearchParameters searchParameters, LinkGenerator linkGenerator)
    {
        var queryable = _dbContext.Trips.AsQueryable().OrderBy(o => o.CreationDate);
        

        var pagedList = await PagedList<Trip>.CreateAsync(queryable, searchParameters.PageNumber!.Value, searchParameters.PageSize!.Value);


        var previousPageLink = pagedList.HasPrevious
            ? linkGenerator.GetUriByName(HttpContext, "GetTrips", new { pageNumber = searchParameters.PageNumber - 1, pageSize = searchParameters.PageSize })
            : null;

        var nextPageLink = pagedList.HasNext
            ? linkGenerator.GetUriByName(HttpContext, "GetTrips", new { pageNumber = searchParameters.PageNumber + 1, pageSize = searchParameters.PageSize })
            : null;

        var paginationMetadata = new PaginationMetadata(pagedList.TotalCount, pagedList.PageSize, pagedList.CurrentPage, pagedList.TotalPages, previousPageLink, nextPageLink);

        HttpContext.Response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationMetadata));

        var trips = pagedList.Select(trip => new TripDtoInitial(trip.Id, trip.Name, trip.Description, trip.Url, trip.TripStart, trip.TripEnd, trip.CreationDate)).ToList();

        return Ok(trips);
    }

    // GET: api/v1/trips/{tripId}
    [HttpGet("{tripId}", Name = "GetTrip")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTripById(int tripId)
    {
        var trip = await _dbContext.Trips
                    .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null)
        {
            return NotFound();
        }

        var tripDto = new TripDtoInitial(trip.Id, trip.Name, trip.Description, trip.Url, trip.TripStart, trip.TripEnd, trip.CreationDate);
        return Ok(tripDto);
    }

    // POST: api/v1/trips
    [HttpPost(Name = "CreateTrip")]
    [Authorize(Roles = TravelRoles.TravelMember)]
    public async Task<IActionResult> CreateTrip([Validate] CreateTripDto createTripDto, LinkGenerator linkGenerator)
    {

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine(string.Join(", ", errors));
            return BadRequest(ModelState);
        }

        var trip = new Trip
        {
            Name = createTripDto.Name,
            Description = createTripDto.Description,
            Url = createTripDto.Url,
            TripStart = createTripDto.TripStart,
            TripEnd = createTripDto.TripEnd,
            CreationDate = DateTime.UtcNow,
            UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
        };

        _dbContext.Trips.Add(trip);
        await _dbContext.SaveChangesAsync();

        var links = CreateLinks(trip.Id, HttpContext, linkGenerator);
        var tripDto = new TripDtoInitial(trip.Id, trip.Name, trip.Description, trip.Url, trip.TripStart, trip.TripEnd, trip.CreationDate);

        var resource = new ResourceDto<TripDtoInitial>(tripDto, links.ToArray());

        return CreatedAtAction(nameof(GetTripById), new { tripId = trip.Id },
            resource);
    }

    // PUT: api/v1/trips/{tripId}
    [HttpPut("{tripId}", Name = "EditTrip")]
    [Authorize]
    public async Task<IActionResult> UpdateTrip(int tripId, [Validate] UpdateTripDto updateTripDto)
    {

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine(string.Join(", ", errors));
            return BadRequest(ModelState);
        }

        var trip = await _dbContext.Trips.FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null)
        {
            return NotFound();
        }

        if (!User.IsInRole(TravelRoles.Admin) && User.FindFirstValue(JwtRegisteredClaimNames.Sub) != trip.UserId)
        {
            return Forbid();
        }

        trip.Name = updateTripDto.Name;
        trip.Description = updateTripDto.Description;
        trip.Url = updateTripDto.Url;
        trip.TripStart = updateTripDto.TripStart;
        trip.TripEnd = updateTripDto.TripEnd;

        _dbContext.Trips.Update(trip);
        await _dbContext.SaveChangesAsync();

        return Ok(new TripDtoInitial(trip.Id, trip.Name, trip.Description, trip.Url, trip.TripStart, trip.TripEnd, trip.CreationDate));
    }

    // DELETE: api/v1/trips/{tripId}
    [HttpDelete("{tripId}", Name = "DeleteTrip")]
    [Authorize]
    public async Task<IActionResult> DeleteTrip(int tripId)
    {
        var trip = await _dbContext.Trips.FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null)
        {
            return NotFound();
        }
        
        if (User.FindFirstValue(JwtRegisteredClaimNames.Sub) != trip.UserId)
        {
            return Forbid();
        }

        _dbContext.Trips.Remove(trip);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    static IEnumerable<LinkDto> CreateLinks(int tripId, HttpContext httpContext, LinkGenerator linkGenerator)
    {
        yield return new LinkDto(linkGenerator.GetUriByName(httpContext, "GetTrip", new { tripId })!, "self", "GET");
        yield return new LinkDto(linkGenerator.GetUriByName(httpContext, "EditTrip", new { tripId })!, "edit", "PUT");
        yield return new LinkDto(linkGenerator.GetUriByName(httpContext, "DeleteTrip", new { tripId })!, "delete", "DELETE");
    }
}