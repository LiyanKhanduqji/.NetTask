using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.DTOs;

public class RegisterDTO
{
    [Required] public string username { get; set; } = string.Empty;
    [Required] public string? KnownAs { get; set; }
    [Required] public string? Gender { get; set; }
    [Required] public string? DateOfBirth { get; set; }
    [Required] public string? City { get; set; }
    [Required] public string? Country { get; set; }

    [Required]
    [StringLength(8, MinimumLength = 4)]
    public string password { get; set; } = string.Empty;

}


// [Required] Attribute tells the validation system that fields must have a value provided by the user (checks for values when data is submitted.)
// (like in a form or API request). If left blank or missing, it will cause a validation error.

//required Keyword is part of the property definition itself. (ensures values are set when the object is created.)
//It enforces that these properties must be assigned a value during object creation. Without setting them, you can't fully create an instance of RegisterDTO.