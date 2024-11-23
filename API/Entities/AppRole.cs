using System;
using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppRole : IdentityRole<int>
{
    // This property establishes a relationship between roles and users
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
}
