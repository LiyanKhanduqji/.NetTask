using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Services;
using API.Interfaces;
using API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

// CORS middleware should be added before mapping controllers to endpoint to work
app.UseCors(x => x.AllowAnyHeader().AllowAnyHeader().WithOrigins("http://localhost:4200", "https://localhost:4200"));


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// to perform database migration and seeding using 'Scoped Service Provider'
//scoped lifetime (like DataContext for Entity Framework) are disposed of automatically when the scope ends. This helps avoid memory leaks.

using var scope = app.Services.CreateScope(); //creates a lifetime scope to ensure that services like DataContext are disposed of correctly after they are used.
var services = scope.ServiceProvider; // is a container that holds all the services registered in the DI container, like DataContext, ILogger

try
{
    var context = services.GetRequiredService<DataContext>(); // retrieves the DataContext (the database context) that has been registered in the DI container.
    await context.Database.MigrateAsync(); //applies any pending migrations to the database
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error accured during migration");
}

app.Run();


