using System;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

// extension methods are static
public static class ApplicationServiceExtensions
{
    // Using this IServiceCollection services allows us to “extend” IServiceCollection with custom methods {builder.Services.AddApplicationServices}
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
    IConfiguration config)
    {
        // Add services to the container.

        services.AddControllers();
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        services.AddCors();
        // use the interface and the implementation class
        services.AddScoped<ITokenService, TokenService>();


        // registers UserRepository as the implementation for IUserRepository with a scoped lifetime
        services.AddScoped<IUserRepository, UserRepository>();

        // part allows AutoMapper to automatically detect all Profile classes across the project, simplifying setup.
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }

}
// services and config are just placeholders for builder.Services and builder.Configuration

// UserRepository will be created for each HTTP request, and it will be used wherever IUserRepository is required in your application.