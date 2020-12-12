using System;
using Microsoft.EntityFrameworkCore;
using WebApplication.Database.Models;
using WebApplication.Helpers;

namespace WebApplication.Database
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.ApplySnakeCase(modelBuilder);
            this.ApplyOnModelCreatingFromAllEntities(modelBuilder);
            
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