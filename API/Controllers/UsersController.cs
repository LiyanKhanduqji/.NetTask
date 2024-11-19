using System;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using AutoMapper;
using API.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using API.Extensions;

namespace API.Controllers;

[Authorize]
//in order to use the repository : remove (DataContext context) and inject the Repo interface
public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : BaseApiController
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

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // if (username == null) return BadRequest("No username found in token"); transfor this to the claims extensions

        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return BadRequest("Couldn't find user");

        mapper.Map(memberUpdateDto, user);
        if (await userRepository.SaveAllAsync()) return NoContent(); // return 200 type response (204)-> means successfully but there is no response

        return BadRequest("Faild to Update the user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return BadRequest("Cannot update user");

        var result = await photoService.AddPhotoAsync(file);
        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if(user.Photos.Count == 0) photo.IsMain =

        user.Photos.Add(photo);

        if (await userRepository.SaveAllAsync())
            return CreatedAtAction(nameof(GetUser),
            new { username = user.userName }, mapper.Map<PhotoDto>(photo));

        return BadRequest("problem accurs when adding photo");
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return BadRequest("Couldn't find user");
        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null || photo.IsMain) return BadRequest("Cannot use this photo as main ");
        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;
        if (await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("problem setting main photo");
    }

    [HttpDelete("delete-photo/{photoId:int}")] // give the id constraint
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return BadRequest("Couldn't find user");
        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null || photo.IsMain) return BadRequest("Couldn't find photo");

        if (photo.PublicId != null)
        {
            var result = await photoService.DeletionResultphotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }
        user.Photos.Remove(photo);
        if (await userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Couldn't delete photo");
    }
}

//ordinary way to create costructor of the controller   
//public UsersController(DataContext context){}