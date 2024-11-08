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

        return services;
    }

}
// services and config are just placeholders for builder.Services and builder.Configuration