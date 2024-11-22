// providing concrete methods for interacting with the User data in the database. 
// The methods in this class use Entity Framework Core to interact with the database asynchronously.
using System;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        return await context.Users
        .Where(x => x.userName == username) // Filters users to match the given username.
        .ProjectTo<MemberDto>(mapper.ConfigurationProvider) // Maps the entity to a MemberDto.
        .SingleOrDefaultAsync(); // Returns the single matching user or null if no match is found.
    }

    public async Task<PagedList<MemberDto>> GetMembersAsyns(UserParams userParams)
    {
        // return await context.Users
        // .Take(5) //Selects the first 5 records from the query result.
        // .Skip(5) // Skips the first 5 records and starts returning data from the 6th record onward.
        // .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
        // .ToListAsync(); now instead of this we will use PagedList Class :

        //Access users yable and ensures only the necessary data (fields in MemberDto) is fetched
        var query = context.Users.AsQueryable();
        query = query.Where(x => x.userName != userParams.CurrentUsername);
        if (userParams.Gender != null)
        {
            query = query.Where(x => x.Gender == userParams.Gender);
        }

        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var MaxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

        query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= MaxDob);
        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created),
            _ => query.OrderByDescending(x => x.LastActive)
        };


        return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);


    }

    // methods returns a Task<AppUser?> because it is asynchronous. If no user is found with the given ID, it will return null
    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        // SingleOrDefaultAsync is a method that returns a single user matching the specified condition. If no user is found, it returns null. 
        // If multiple users match the condition (which ideally shouldn't happen), it will throw an exception.

        // This query is different from the FindAsync because it’s not using the primary key (id). Instead, it searches based on a non-primary key field (userName). This makes it less optimized than FindAsync and involves querying the database with a condition.
        return await context.Users.
        Include(x => x.Photos). // exiplicity tell the method to return related entity
        SingleOrDefaultAsync(x => x.userName == username);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await context.Users.
        Include(x => x.Photos). // exiplicity tell the method to return related entity
        ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        // returns whether the save was successful.
        // If the number of changes saved is greater than 0, it returns true, indicating that changes were saved successfully. Otherwise, it returns false.
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        // This tells EF Core that the given user has been modified, and it should update the corresponding record in the database when SaveChangesAsync is called.
        context.Entry(user).State = EntityState.Modified;
    }
}
