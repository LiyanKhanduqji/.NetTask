using System;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface ILikeRepository
{
    Task<UserLike?> GetUserLike(int SourceUserId, int TargetUserId);
    Task<IEnumerable<MemberDto>> GetUserLikes(string predicate, int userId);
    Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
    void DeleteLike(UserLike like);
    Task<bool> SaveChanges();
}
