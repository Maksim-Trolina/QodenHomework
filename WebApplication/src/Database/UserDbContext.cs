using System;
using System.Collections.Generic;
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

        public DbSet<Account> Accounts { get; set; }

        public DbSet<CurrencyAll> CurrencyNames { get; set; }
        
        public DbSet<CurrencyUser> CurrencyUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*this.ApplySnakeCase(modelBuilder);
            this.ApplyOnModelCreatingFromAllEntities(modelBuilder);*/

            /*modelBuilder.Entity<User>()
                .HasMany(u => u.Accounts)
                .WithOne(ac => ac.User)
                .HasForeignKey(ac => ac.UserId)
                .HasPrincipalKey(u => u.Id);*/
            
            
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "Admin",
                    Password = "Admin",
                    Role = "Admin"
                });

            modelBuilder.Entity<CurrencyAll>().HasData(
                new CurrencyAll
                {
                    Id = "bucks"
                });
        }
    }
}