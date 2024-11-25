using System;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services,
       IConfiguration config)
    {
        //sets up identity services
        services.AddIdentityCore<AppUser>(opt =>
         {
             opt.Password.RequireNonAlphanumeric = false;
         })
             .AddRoles<AppRole>()
             .AddRoleManager<RoleManager<AppRole>>()
             .AddEntityFrameworkStores<DataContext>();



        // Define Authentication Scheme
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => //Configures how the application will handle and validate JWT tokens using specific options.
        {
            // token key should be the same exact as encrypting cause is symmetric key
            var tokenKey = config["TokenKey"] ?? throw new Exception("Token not found");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true, // without this all tokens will be accepted without validation
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
            .AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
        return services;
    }
}
