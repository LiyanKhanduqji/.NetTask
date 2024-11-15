using System;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByUsernameAsync(string username);
    Task<IEnumerable<MemberDto>> GetMembersAsyns();
    Task<MemberDto?> GetMemberAsync(string username);
}

// The Task class is a part of the Task Parallel Library (TPL), which allows you to perform operations asynchronously and efficiently.
// Task<T> is used for operations that run asynchronously, meaning they don't block the main thread while waiting for the operation to finish

// Task<IEnumerable<AppUser>> means that the method will return a task that, when completed, will contain an IEnumerable<AppUser>.
// In other words, the method will eventually give you a collection (like a list) of AppUser objects, but it will do so asynchronously.