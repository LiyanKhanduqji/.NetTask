using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDTO
{
    [Required]
    public required string username {get; set;}

    [Required]
    public required string password {get; set;}

}


// [Required] Attribute tells the validation system that fields must have a value provided by the user (checks for values when data is submitted.)
// (like in a form or API request). If left blank or missing, it will cause a validation error.

//required Keyword is part of the property definition itself. (ensures values are set when the object is created.)
//It enforces that these properties must be assigned a value during object creation. Without setting them, you can't fully create an instance of RegisterDTO.