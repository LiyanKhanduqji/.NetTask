// Seeding script that populates the database with initial user data if the database is empty.
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {
        // If there are already users in the database, it stops (returns) to avoid adding duplicate data
        if (await context.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        // Converts (deserializes) the JSON data into a list of AppUser objects.
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null) return;
        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();

            // Ensures consistency in how usernames are stored.
            user.userName = user.userName.ToLower();

            // Sets each user’s password hash (PasswordHash) and salt (PasswordSalt) based on the text "Pa$$w0rd"
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            user.PasswordSalt = hmac.Key;

            // Adds each user to the Users table in the databas
            context.Users.Add(user);
        }

        // saves all added users to the database.
        await context.SaveChangesAsync();

    }
}