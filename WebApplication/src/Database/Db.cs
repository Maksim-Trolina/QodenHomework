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

        }
        
        public DbSet<Account> Accounts { get; set; }

        public DbSet<CurrencyAll> CurrencyNames { get; set; }
        
        public DbSet<CurrencyUser> CurrencyUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasKey(u => u.UserMail);

            modelBuilder.Entity<CurrencyAll>()
                .HasKey(u => u.CurrencyName);

            modelBuilder.Entity<CurrencyUser>()
                .HasKey(u => u.UserName);
        }
    }
}