using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Auth.Model;
using System.Text.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using TravelPlannerAPI.Data.Dto;
using TravelPlannerAPI.Helpers;

namespace TravelPlannerAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly TravelDbContext _dbContext;
        private readonly UserManager<TravelMember> _userManager;

        public UserController(TravelDbContext dbContext, UserManager<TravelMember> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        // GET: api/v1/users
        // /users?pageNumber=1&pageSize=5
        [HttpGet(Name = "GetUsers")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetUsers([FromQuery] SearchParameters searchParameters, LinkGenerator linkGenerator)
        {
            var queryable = _dbContext.Users.AsQueryable().OrderBy(u => u.UserName);
            
            var pagedList = await PagedList<TravelMember>.CreateAsync(queryable, searchParameters.PageNumber!.Value, searchParameters.PageSize!.Value);
            
            var previousPageLink = pagedList.HasPrevious
                ? linkGenerator.GetUriByName(HttpContext, "GetUsers", new { pageNumber = searchParameters.PageNumber - 1, pageSize = searchParameters.PageSize })
                : null;

            var nextPageLink = pagedList.HasNext
                ? linkGenerator.GetUriByName(HttpContext, "GetUsers", new { pageNumber = searchParameters.PageNumber + 1, pageSize = searchParameters.PageSize })
                : null;

            var paginationMetadata = new PaginationMetadata(pagedList.TotalCount, pagedList.PageSize, pagedList.CurrentPage, pagedList.TotalPages, previousPageLink, nextPageLink);
            
            HttpContext.Response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationMetadata));
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            
            var members = new List<TravelMemberDto.TravelMemberDtoInitial>();

            foreach (var member in pagedList)
            {
                var roles = await _userManager.GetRolesAsync(member);
                
                var memberDto = new TravelMemberDto.TravelMemberDtoInitial(
                    member.Id,
                    member.UserName,
                    member.Email,
                    string.Join(", ", roles)
                );

                members.Add(memberDto);
            }

            return Ok(members);
        }
        
        [HttpPost("assignAdmin/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignAdminRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            
            var isInRole = await _userManager.IsInRoleAsync(user, "Admin");
            if (isInRole)
            {
                return BadRequest(new { message = "User is already an Admin" });
            }
            
            var result = await _userManager.AddToRoleAsync(user, "Admin");
            
            if (!result.Succeeded)
            {
                return StatusCode(500, new { message = "Failed to assign Admin role" });
            }

            return Ok(new { message = "User role updated to Admin" });
        }
        
        [HttpPost("removeAllRoles/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveAllRolesAndSetToTravelMember(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            
            var userRoles = await _userManager.GetRolesAsync(user);
            
            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (!removeResult.Succeeded)
            {
                return StatusCode(500, new { message = "Failed to remove roles" });
            }
            
            var addResult = await _userManager.AddToRoleAsync(user, "TravelMember");

            if (!addResult.Succeeded)
            {
                return StatusCode(500, new { message = "Failed to add TravelMember role" });
            }

            return Ok(new { message = "User roles removed and set to TravelMember" });
        }
        
        [HttpDelete("deleteUser/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(500, new { message = "Failed to delete user" });
            }

            return Ok(new { message = "User deleted successfully" });
        }
    }
}
