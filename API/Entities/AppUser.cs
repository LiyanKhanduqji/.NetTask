using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppUser
{
    public int Id {get; set;}
    public required string userName {get; set;}

}
