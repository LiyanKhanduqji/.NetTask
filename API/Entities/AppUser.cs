using System;
using System.ComponentModel.DataAnnotations;
using API.Extensions;

namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public required string userName { get; set; }
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public DateOnly DateOfBirth { get; set; }
    public required string KnownAs { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public required string Gender { get; set; }
    public string? Introduction { get; set; }
    public string? Skills { get; set; }
    public string? LookingFor { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public List<Photo> Photos { get; set; } = []; // navigation property

    // we will add another approach in the autoMApperprofiles
    // public int GetAge()
    // {
    //     return DateOfBirth.CalculateAge();
    // } this force us to retrieve the passhash and passsalt every time cause it needs to get appuser entity before calculationg cause DOB doesn't exist in the member dto

}

//A navigation property links entities to their related entities in a database model, reflecting relationships