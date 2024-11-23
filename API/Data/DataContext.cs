// DataContext file in Entity Framework (EF) serves as the bridge between the application’s code and the database. It inherits from DbContext
using System;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

// This represents peimary constructor 
public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>(options)
{
        public DbSet<UserLike>? Likes { get; set; }

        // method defines the many-to-many relationship between AppUser and UserLike
        protected override void OnModelCreating(ModelBuilder builder)
        {
                base.OnModelCreating(builder);

                builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles) //Specifies that each AppUser can have many AppUserRole
                .WithOne(u => u.User) // each record in the AppUserRole table is linked to exactly one AppUser
                .HasForeignKey(ur => ur.UserId) // Defines that the UserId property in AppUserRole is the foreign key referencing the AppUser table.
                .IsRequired(); // a user must have associated roles

                builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

                // defines a primary key combining SourceUserId and TargetUserId. This ensures each "like" is unique
                builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.TargetUserId });

                builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade); //ensures likes are deleted if the user is removed

                builder.Entity<UserLike>()
                .HasOne(s => s.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
}



// ordinary way to define the constructor in order to pass optione to the parent class : 
// public DataContext(DbContextOptions options) : base(options){}


//by defining DbSet<AppUser> Users in your DataContext, you’re telling Entity Framework (EF) to create a table named Users in the database, which will store data for AppUser entities


// seperated dbset didn't be added cause photos are included as a navigation property within the AppUser entity
// photo dbset lets you query Photos directly, but it's not required if photos are mostly accessed as part of user profiles. (public DbSet<Photo> Photos { get; set; })
// in case we didn't define seperated dbset for photoes, table name will be the photos entity class in 'Photo.cs file'


// DataContext file purposes:

// 1 -Defining Database Tables:
// DbSet<T> properties (e.g., DbSet<AppUser> Users) represent database tables for each entity type (AppUser).

//2- Configuring Relationships:
// It defines relationships between entities through navigation properties

//3- Managing Data Access:
// It enables querying, adding, updating, and deleting data in the database through LINQ.

// 4- Tracking Changes:
// EF tracks changes to entities within the DataContext, allowing for data updates when SaveChanges() is called.

// 5- Managing Database Creation/Migrations:
// It’s the central point for database schema creation and migrations in EF Code First.