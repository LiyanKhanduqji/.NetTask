using System;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using AutoMapper;
using API.DTOs;

namespace API.Controllers;

[Authorize]
//in order to use the repository : remove (DataContext context) and inject the Repo interface
public class UsersController(IUserRepository userRepository) : BaseApiController
{

    //allow access to a login or register without requiring authentication
    // [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        // var users = await context.Users.ToListAsync(); instead of this use:
        // var users = await userRepository.GetUsersAsync(); instead of this 
        var users = await userRepository.GetMembersAsyns();

        // to use automapper
        // var usersToReturn = mapper.Map<IEnumerable<MemberDto>>(users); no need for this cause the dto is filtered in the db level
        return Ok(users);
    }

    // [HttpGet("{Id:int}")] instead of this use:
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {

        // var user = await context.Users.FindAsync(id); instead of this use:
        // var user = await userRepository.GetUserByUsernameAsync(username); instead of this:
        var user = await userRepository.GetMemberAsync(username);

        if (user == null) return NotFound();
        // return mapper.Map<MemberDto>(user);
        return user;
    }
}

//ordinary way to create costructor of the controller   
//public UsersController(DataContext context){}