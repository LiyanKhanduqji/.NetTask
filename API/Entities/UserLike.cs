using System;

namespace API.Entities;

public class UserLike
{
    public AppUser SourceUser { get; set; } = null!; //null-forgiving operator (!) 
    public int SourceUserId { get; set; }
    public AppUser TargetUser { get; set; } = null!;
    public int TargetUserId { get; set; }
}
