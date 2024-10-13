using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using O9d.AspNet.FluentValidation;
using static Trip;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TravelDbContext>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateActivityDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateActivityDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateDestinationDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateDestinationDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTripDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateTripDtoValidator>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
