using System;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // this means that the cotroller will be replaced with the first part of class name
public class UsersController(DataContext context) : ControllerBase // localhost/api/users
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{Id:int}")]
    public async Task<ActionResult<AppUser>> GetUser(int id){

        var user = await context.Users.FindAsync(id);

        if(user == null) return NotFound();
        return user;
    }
}

//ordinary way to create costructor of the controller   
//public UsersController(DataContext context){}