using Microsoft.AspNetCore.Identity;
using TravelPlannerAPI.Auth.Model;

namespace TravelPlannerAPI.Auth;

public static class AuthEndpoints
{
    public static void AddAuthApi(this WebApplication app)
{
    // register
    app.MapPost("api/accounts", async (UserManager<TravelMember> userManager, IServiceProvider serviceProvider, RegisterUserDto dto) =>
    {
        // Check if user exists
        var user = await userManager.FindByNameAsync(dto.UserName);
        if (user != null)
        {
            return Results.UnprocessableEntity("Username already taken");
        }

        // Create a new user object
        var newUser = new TravelMember()
        {
            Email = dto.Email,
            UserName = dto.UserName
        };

        // Begin transaction using a DbContext
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TravelDbContext>();

            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var createUserResult = await userManager.CreateAsync(newUser, dto.Password);
                    if (!createUserResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return Results.UnprocessableEntity("Failed to create user.");
                    }
                    
                    var addRoleResult = await userManager.AddToRoleAsync(newUser, TravelRoles.TravelMember);
                    if (!addRoleResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return Results.UnprocessableEntity("Failed to assign role.");
                    }
                    
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Results.UnprocessableEntity($"An error occurred: {ex.Message}");
                }
            }
        }
        // Return success response if everything worked
        return Results.Created();
    });
    
    // login
    app.MapPost("api/login", async (UserManager<TravelMember> userManager, JwtTokenService jwtTokenService, LoginDto dto) =>
    {
        // Check if user exists
        var user = await userManager.FindByNameAsync(dto.UserName);
        if (user == null)
        {
            return Results.UnprocessableEntity("User does not exist");
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, dto.Password);
        if (!isPasswordValid)
        {
            Results.UnprocessableEntity("Username or password was incorrect.");
        }

        var roles = await userManager.GetRolesAsync(user);

        var accessToken = jwtTokenService.CreateAcessToken(user.UserName, user.Id, roles);
        
        return Results.Ok(new SuccesfulLoginDto(accessToken));
    });
}
    
    public record RegisterUserDto(string UserName, string Email, string Password);
    public record LoginDto(string UserName, string Password);

    public record SuccesfulLoginDto(string AccessToken);
}