using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using AutoMapper;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService, IMapper mapper) : BaseApiController
{
    [HttpPost("register")] // the endpoint will be : account/register

    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
    {


        if (await UserExists(registerDTO.username)) return BadRequest("User name is alraedy taken");
        using var hmac = new HMACSHA512();

        var user = mapper.Map<AppUser>(registerDTO);

        user.UserName = registerDTO.username.ToLower();

        // var user = new AppUser
        // {
        //     userName = registerDTO.username.ToLower(),
        //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.password)),
        //     PasswordSalt = hmac.Key
        // };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserDTO
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }

    private async Task<bool> UserExists(string username)
    {
        //x represents each individual user in the Users collection within context
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }


    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
    {

        // Users.Where : list of users with specific criteria
        // Users.FirstOrDefaultAsync : if the user not found, return default value (null)
        // Users.FirstAsync : throw exception if the user doesn't exist
        // Users.SingleOrDefaultAsync : find only exist users, and throw exception if there is more than one element matches
        var user = await context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.UserName == loginDTO.username.ToLower());

        if (user == null) return Unauthorized("Invalid username");

        return new UserDTO
        {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Token = tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            Gender = user.Gender
        };
    }
}
