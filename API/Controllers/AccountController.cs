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
using Microsoft.AspNetCore.Identity;

namespace API.Controllers;

public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper) : BaseApiController
{
    [HttpPost("register")] // the endpoint will be : account/register

    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
    {


        if (await UserExists(registerDTO.username)) return BadRequest("User name is alraedy taken");

        var user = mapper.Map<AppUser>(registerDTO);

        user.UserName = registerDTO.username.ToLower();

        var result = await userManager.CreateAsync(user, registerDTO.password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        // var user = new AppUser
        // {
        //     userName = registerDTO.username.ToLower(),
        //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.password)),
        //     PasswordSalt = hmac.Key
        // };

        return new UserDTO
        {
            Username = user.UserName,
            Token = await tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }

    private async Task<bool> UserExists(string username)
    {
        //x represents each individual user in the Users collection within context
        return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper());

        // ASP.NET Identity adds normalized versions of certain fields like the username and email (NormalizedUserName and NormalizedEmail). These fields are pre-stored in an uppercase format in the database
    }


    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
    {

        // Users.Where : list of users with specific criteria
        // Users.FirstOrDefaultAsync : if the user not found, return default value (null)
        // Users.FirstAsync : throw exception if the user doesn't exist
        // Users.SingleOrDefaultAsync : find only exist users, and throw exception if there is more than one element matches
        var user = await userManager.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.NormalizedUserName == loginDTO.username.ToUpper());

        if (user == null || user.UserName == null) return Unauthorized("Invalid username");

        var result = await userManager.CheckPasswordAsync(user, loginDTO.password);

        if (!result) return Unauthorized();

        return new UserDTO
        {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Token = await tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            Gender = user.Gender
        };
    }
}
