using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;


// using annotation to name the table Photos
[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }

    // to set up a required one to many relationship: Navigation Properties
    public int AppUserId { get; set; } //Foreign Key links each Photo record to a specific AppUser

    // When querying a Photo, the AppUser navigation property lets you access the details of the AppUser who owns the photo.
    // By marking AppUser as non-nullable (null!;), it ensures that each Photo must have an associated AppUser, establishing the required one-to-many relationship.
    public AppUser AppUser { get; set; } = null!; //reference navigation property. It links each Photo to its associated AppUser entity.
}
//The foreign key ensures that each photo is tied to a specific user in the database, enforcing referential integrity. 
// It allows EF to know which AppUser each Photo belongs to.