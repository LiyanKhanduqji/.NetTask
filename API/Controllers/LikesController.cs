using System;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController(ILikeRepository LikesRepository) : BaseApiController
{
    [HttpPost("{targetUserId:int}")]
    public async Task<ActionResult> ToggleLike(int targetUserId)
    {
        var sourceUserId = User.GetUserId(); //Retrieve the ID of the current user
        if (sourceUserId == targetUserId) return BadRequest("You can't like tourself!");

        var existingLike = await LikesRepository.GetUserLike(sourceUserId, targetUserId);
        if (existingLike == null)
        {
            var like = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = targetUserId,
            };

            LikesRepository.AddLike(like);
        }
        else
        {
            LikesRepository.DeleteLike(existingLike);
        }
        if (await LikesRepository.SaveChanges()) return Ok();
        return BadRequest("Failed to update like");
    }


    // Fetches a list of user IDs that the current user has liked
    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
    {
        return Ok(await LikesRepository.GetCurrentUserLikeIds(User.GetUserId()));
    }

    [HttpGet]
    // A predicate is a condition used to determine what kind of data to fetch
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes(string predicate)
    {
        var users = await LikesRepository.GetUserLikes(predicate, User.GetUserId());
        return Ok(users);
    }
}

// BaseApiController: Provides a base route and shared functionality for all controllers

// A predicate is a condition or filter used to determine the data to retrieve. In this case:
// predicate="liked" → Fetch users the current user has liked.
// predicate="likedBy" → Fetch users who liked the current user.