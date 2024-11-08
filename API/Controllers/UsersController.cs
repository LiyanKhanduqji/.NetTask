using System;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;


public class UsersController(DataContext context) : BaseApiController
{

    //allow access to a login or register without requiring authentication
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{Id:int}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {

        var user = await context.Users.FindAsync(id);

        if (user == null) return NotFound();
        return user;
    }
}

//ordinary way to create costructor of the controller   
//public UsersController(DataContext context){}