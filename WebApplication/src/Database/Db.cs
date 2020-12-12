using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication.Database.Models;

namespace WebApplication.Database
{
    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options)
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