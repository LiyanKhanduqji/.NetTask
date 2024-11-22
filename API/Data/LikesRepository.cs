using System;
using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Data;

public class LikesRepository : ILikeRepository
{
    public void DeleteLike(UserLike like)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId)
    {
        throw new NotImplementedException();
    }

    public Task<UserLike?> GetUserLike(int SourceUserId, int TargetUserId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MemberDto>> GetUserLikes(string predicate, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveChanges()
    {
        throw new NotImplementedException();
    }
}
