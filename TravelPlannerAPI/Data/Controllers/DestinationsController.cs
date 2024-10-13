using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using O9d.AspNet.FluentValidation;
using TravelPlannerAPI.Helpers;
using static DestinationDto;

[ApiController]
[Route("api/v1/trips/{tripId}/[controller]")]
public class DestinationsController : ControllerBase
{
    private readonly TravelDbContext _dbContext;
    public DestinationsController(TravelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: api/v1/trips/{tripId}/destinations
    [HttpGet(Name = "GetDestinations")]
    public async Task<IActionResult> GetDestinations([FromQuery] SearchParameters searchParameters, LinkGenerator linkGenerator, CancellationToken cancellationToken)
    {
        var queryable = _dbContext.Destinations.AsQueryable().OrderBy(o => o.CreationDate);

        var pagedList = await PagedList<Destination>.CreateAsync(queryable, searchParameters.PageNumber!.Value, searchParameters.PageSize!.Value);

        var previousPageLink = pagedList.HasPrevious
            ? linkGenerator.GetUriByName(HttpContext, "GetDestinations", new { pageNumber = searchParameters.PageNumber - 1, pageSize = searchParameters.PageSize })
            : null;

        var nextPageLink = pagedList.HasNext
            ? linkGenerator.GetUriByName(HttpContext, "GetDestinations", new { pageNumber = searchParameters.PageNumber + 1, pageSize = searchParameters.PageSize })
            : null;

        var paginationMetadata = new PaginationMetadata(pagedList.TotalCount, pagedList.PageSize, pagedList.CurrentPage, pagedList.TotalPages, previousPageLink, nextPageLink);

        HttpContext.Response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationMetadata));

        var destinations = pagedList.Select(destination => new DestinationDtoInitial(destination.Id, destination.StartLocation, destination.EndLocation, destination.CreationDate)).ToList();

        return Ok(destinations);
    }

    // GET: api/v1/trips/{tripId}/destinations/{destinationId}
    [HttpGet("{destinationId}", Name = "GetDestination")]
    public async Task<IActionResult> GetDestinationById(int tripId, int destinationId)
    {
        var destination = await _dbContext.Destinations
                    .FirstOrDefaultAsync(d => d.Id == destinationId && d.Trip.Id == tripId);

        if (destination == null)
        {
            return NotFound();
        }

        var destinationDto = new DestinationDtoInitial(destination.Id, destination.StartLocation, destination.EndLocation, destination.CreationDate);
        return Ok(destinationDto);
    }

    // POST: api/v1/trips/{tripId}/destinations
    [HttpPost(Name = "CreateDestination")]
    public async Task<IActionResult> CreateDestination(int tripId, [Validate] CreateDestinationDto createDestinationDto, LinkGenerator linkGenerator)
    {

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine(string.Join(", ", errors));
            return BadRequest(ModelState);
        }

        var trip = await _dbContext.Trips.FindAsync(tripId);
        if (trip == null)
        {
            return NotFound("Trip not found");
        }

        var destination = new Destination
        {
            StartLocation = createDestinationDto.StartLocation,
            EndLocation = createDestinationDto.EndLocation,
            CreationDate = DateTime.UtcNow,
            Trip = trip
        };

        _dbContext.Destinations.Add(destination);
        await _dbContext.SaveChangesAsync();

        var links = CreateLinks(tripId, destination.Id, HttpContext, linkGenerator);
        var destinationDto = new DestinationDtoInitial(destination.Id, destination.StartLocation, destination.EndLocation, destination.CreationDate);

        var resource = new ResourceDto<DestinationDtoInitial>(destinationDto, links.ToArray());

        return CreatedAtAction(nameof(GetDestinationById), new { tripId = trip.Id, destinationId = destination.Id },
           resource);
    }

    // PUT: api/v1/trips/{tripId}/destinations/{destinationId}
    [HttpPut("{destinationId}", Name = "EditDestination")]
    public async Task<IActionResult> UpdateDestination(int tripId, int destinationId, [Validate] UpdateDestinationDto updateDestinationDto)
    {

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine(string.Join(", ", errors)); // Use your logging framework
            return BadRequest(ModelState);
        }

        var destination = await _dbContext.Destinations
            .FirstOrDefaultAsync(d => d.Id == destinationId && d.Trip.Id == tripId);

        if (destination == null)
        {
            return NotFound();
        }

        destination.StartLocation = updateDestinationDto.StartLocation;
        destination.EndLocation = updateDestinationDto.EndLocation;

        _dbContext.Destinations.Update(destination);
        await _dbContext.SaveChangesAsync();

        return Ok(new DestinationDtoInitial(destination.Id, destination.StartLocation, destination.EndLocation, destination.CreationDate));
    }

    // DELETE: api/v1/trips/{tripId}/destinations/{destinationId}
    [HttpDelete("{destinationId}", Name = "DeleteDestination")]
    public async Task<IActionResult> DeleteDestination(int tripId, int destinationId)
    {
        var destination = await _dbContext.Destinations
            .FirstOrDefaultAsync(d => d.Id == destinationId && d.Trip.Id == tripId);

        if (destination == null)
        {
            return NotFound();
        }

        _dbContext.Destinations.Remove(destination);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    static IEnumerable<LinkDto> CreateLinks(int tripId, int destinationId, HttpContext httpContext, LinkGenerator linkGenerator)
    {
        yield return new LinkDto(linkGenerator.GetUriByName(httpContext, "GetDestination", new { tripId, destinationId })!, "self", "GET");
        yield return new LinkDto(linkGenerator.GetUriByName(httpContext, "EditDestination", new { tripId, destinationId })!, "edit", "PUT");
        yield return new LinkDto(linkGenerator.GetUriByName(httpContext, "DeleteDestination", new { tripId, destinationId })!, "delete", "DELETE");
    }
}