// Seeding script that populates the database with initial user data if the database is empty.
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(UserManager<AppUser> userManager)
    {
        // If there are already users in the database, it stops (returns) to avoid adding duplicate data
        if (await userManager.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        // Converts (deserializes) the JSON data into a list of AppUser objects.
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null) return;
        foreach (var user in users)
        {
           await userManager.CreateAsync(user, "Liyan22Pa$$w0rd22");
        }

    }
}