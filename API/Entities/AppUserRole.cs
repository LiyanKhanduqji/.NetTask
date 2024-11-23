using System;
using Microsoft.AspNetCore.Identity;

namespace API.Entities;

// The <int> specifies that the primary key (ID) for roles will be of type int, instead of the default string
// By inheriting IdentityUserRole<int>, AppUserRole automatically gets UserId and RoleId as properties (foreign keys) that refer to the AppUser and AppRole tables, respectively.
public class AppUserRole : IdentityUserRole<int>
{
    // navigation properties (User and Role), which allow you to access the AppUser and AppRole entities associated with a specific AppUserRole
    public AppUser User { get; set; } = null!;
    public AppRole Role { get; set; } = null!;
}
