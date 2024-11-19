using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // CreateMap<From, To>();
        CreateMap<AppUser, MemberDto>()
        .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
        .ForMember(d => d.PhotoUrl, o =>
        o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        CreateMap<Photo, PhotoDto>();
        CreateMap<MemberUpdateDto, AppUser>();
        CreateMap<RegisterDTO, AppUser>();
        CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
    }
}

//s.Photos:
// Refers to the Photos collection in the AppUser class.

// FirstOrDefault(x => x.IsMain):
// Searches the Photos collection for the first photo where the IsMain property is true.
// If no such photo exists, it returns null.

// !.Url:
// Retrieves the Url property of the found photo.
// The ! (null-forgiveness operator) is used to tell the compiler that you are confident the result will not be null soo it will not throw an exception