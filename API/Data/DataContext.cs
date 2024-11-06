using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

// This represents peimary constructor 
public class DataContext(DbContextOptions options) : DbContext(options) 
{
        public DbSet<AppUser> Users {get; set;}
}


// ordinary way to define the constructor in order to pass optione to the parent class : 
// public DataContext(DbContextOptions options) : base(options){}
