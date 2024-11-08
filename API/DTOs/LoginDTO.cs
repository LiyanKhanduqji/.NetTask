using System;

namespace API.DTOs;

public class LoginDTO
{
    public required string username { get; set; }
    public required string password { get; set; }

}
