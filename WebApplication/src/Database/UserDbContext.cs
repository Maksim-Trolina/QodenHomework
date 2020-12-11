using System;
using Microsoft.EntityFrameworkCore;
using WebApplication.Database.Models;

namespace WebApplication.Database
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "Admin",
                    Password = "Admin",
                    Role = "Admin"
                });
        }
    }
}