// DataContext file in Entity Framework (EF) serves as the bridge between the application’s code and the database. It inherits from DbContext
using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

// This represents peimary constructor 
public class DataContext(DbContextOptions options) : DbContext(options)
{
        public DbSet<AppUser> Users { get; set; }
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