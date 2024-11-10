using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using O9d.AspNet.FluentValidation;
using TravelPlannerAPI.Helpers;
using static ActivityDto;

[ApiController]
[Route("api/v1/trips/{tripId}/destinations/{destinationId}/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly TravelDbContext _dbContext;
    public ActivitiesController(TravelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: api/v1/trips/{tripId}/destinations/{destinationId}/activities
    [HttpGet(Name = "GetActivities")]
    public async Task<IActionResult> GetActivities([FromQuery] SearchParameters searchParameters, LinkGenerator linkGenerator, CancellationToken cancellationToken)
    {
        var queryable = _dbContext.Activities.AsQueryable().OrderBy(o => o.CreationDate);

        var pagedList = await PagedList<Activity>.CreateAsync(queryable, searchParameters.PageNumber!.Value, searchParameters.PageSize!.Value);

        var previousPageLink = pagedList.HasPrevious
            ? linkGenerator.GetUriByName(HttpContext, "GetActivities", new { pageNumber = searchParameters.PageNumber - 1, pageSize = searchParameters.PageSize })
            : null;

        var nextPageLink = pagedList.HasNext
            ? linkGenerator.GetUriByName(HttpContext, "GetActivities", new { pageNumber = searchParameters.PageNumber + 1, pageSize = searchParameters.PageSize })
            : null;

        
        var paginationMetadata = new PaginationMetadata(pagedList.TotalCount, pagedList.PageSize, pagedList.CurrentPage, pagedList.TotalPages, previousPageLink, nextPageLink);

        HttpContext.Response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationMetadata));

        var activities = pagedList.Select(activity => new ActivityDtoInitial(activity.Id, activity.Name, activity.Type, activity.StartTime, activity.EndTime, activity.CreationDate)).ToList();
        
        return Ok(activities);
    }

    // GET: api/v1/trips/{tripId}/destinations/{destinationId}/activities/{activitiesId}
    [HttpGet("{activitiesId}", Name = "GetActivity")]
    public async Task<IActionResult> GetActivityById(int tripId, int destinationId, int activitiesId)
    {
        var activity = await _dbContext.Activities
                    .FirstOrDefaultAsync(a => a.Id == activitiesId && a.Destination.Id == destinationId && a.Destination.Trip.Id == tripId);

        if (activity == null)
        {
            return NotFound();
        }

        var activityDto = new ActivityDtoInitial(activity.Id, activity.Name, activity.Type, activity.StartTime, activity.EndTime, activity.CreationDate);
        return Ok(activityDto);
    }

    // POST: api/v1/trips/{tripId}/destinations/{destinationId}/activities
    [HttpPost(Name = "CreateActivity")]
    public async Task<IActionResult> CreateActivity(int tripId, int destinationId, [Validate] CreateActivityDto createActivityDto, LinkGenerator linkGenerator)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine(string.Join(", ", errors)); // Use your logging framework
            return BadRequest(ModelState);
        }


        var trip = await _dbContext.Trips.FindAsync(tripId);
        if (trip == null)
        {
            return NotFound("Trip not found");
        }

        var destination = await _dbContext.Destinations.FindAsync(destinationId);
        if (destination == null)
        {
            return NotFound("Destination not found");
        }

        var activity = new Activity
        {
            Name = createActivityDto.Name,
            Type = createActivityDto.Type,
            StartTime = createActivityDto.StartTime,
            EndTime = createActivityDto.EndTime,
            CreationDate = DateTime.UtcNow,
            Destination = destination,
            UserId = ""
        };

        _dbContext.Activities.Add(activity);
        await _dbContext.SaveChangesAsync();
        
        var links = CreateLinks(tripId, destinationId, activity.Id, HttpContext, linkGenerator);
        var activityDto = new ActivityDtoInitial(activity.Id, activity.Name, activity.Type, activity.StartTime, activity.EndTime, activity.CreationDate);

        var resource = new ResourceDto<ActivityDtoInitial>(activityDto, links.ToArray());

        // Ensure to match the parameter name in the CreatedAtAction
        return CreatedAtAction(nameof(GetActivityById), new { tripId = trip.Id, destinationId = destination.Id, activitiesId = activity.Id },
            resource);
    }


    // PUT: api/v1/trips/{tripId}/destinations/{destinationId}/activities/{activitiesId}
    [HttpPut("{activitiesId}", Name = "EditActivity")]
    public async Task<IActionResult> UpdateActivity(int tripId, int destinationId, int activitiesId, [Validate] UpdateActivityDto updateActivityDto)
    {

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine(string.Join(", ", errors)); // Use your logging framework
            return BadRequest(ModelState);
        }

        var activity = await _dbContext.Activities
            .FirstOrDefaultAsync(a => a.Id == activitiesId && a.Destination.Id == destinationId && a.Destination.Trip.Id == tripId);

        if (activity == null)
        {
            return NotFound();
        }

        activity.Name = updateActivityDto.Name;
        activity.Type = updateActivityDto.Type;
        activity.StartTime = updateActivityDto.StartTime;
        activity.EndTime = updateActivityDto.EndTime;

        _dbContext.Activities.Update(activity);
        await _dbContext.SaveChangesAsync();

        return Ok(new ActivityDtoInitial(activity.Id, activity.Name, activity.Type, activity.StartTime, activity.EndTime, activity.CreationDate));
    }

    // DELETE: api/v1/trips/{tripId}/destinations/{destinationId}/activities/{activitiesId}
    [HttpDelete("{activitiesId}", Name = "DeleteActivity")]
    public async Task<IActionResult> DeleteAcitivty(int tripId, int destinationId, int activitiesId)
    {
        var activity = await _dbContext.Activities
            .FirstOrDefaultAsync(a => a.Id == activitiesId && a.Destination.Id == destinationId && a.Destination.Trip.Id == tripId);

        if (activity == null)
        {
            return NotFound();
        }

        _dbContext.Activities.Remove(activity);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    static IEnumerable<LinkDto> CreateLinks(int tripId, int destinationId, int activitiesId, HttpContext httpContext, LinkGenerator linkGenerator)
    {
        yield return new LinkDto(linkGenerator.GetUriByName(httpContext, "GetActivity", new { tripId, destinationId, activitiesId })!, "self", "GET");
        yield return new LinkDto(linkGenerator.GetUriByName(httpContext, "EditActivity", new { tripId, destinationId, activitiesId })!, "edit", "PUT");
        yield return new LinkDto(linkGenerator.GetUriByName(httpContext, "DeleteActivity", new { tripId, destinationId, activitiesId })!, "delete", "DELETE");
    }
}