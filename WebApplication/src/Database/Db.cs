using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication.Database.Models;
using WebApplication.Helpers;

namespace WebApplication.Database
{
    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options)
            : base(options)
        {

        }
        
        public DbSet<Account> Accounts { get; set; }

        public DbSet<CurrencyAll> CurrencyAlls { get; set; }
        
        public DbSet<CurrencyUser> CurrencyUsers { get; set; }

        public DbSet<CurrencyAccount> CurrencyAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetInternalKeys(modelBuilder);
            
            CreateDefaultUsers(modelBuilder);
        }

        private void SetInternalKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasKey(u => u.AccountName);

            modelBuilder.Entity<CurrencyAll>()
                .HasKey(u => u.CurrencyName);

            modelBuilder.Entity<CurrencyUser>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<CurrencyAccount>()
                .HasKey(u => u.Id);
        }

        private void CreateDefaultUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasData(new Account
                    {UserMail = "Admin", AccountName = "Admin", Password = "Admin", Role = (byte) Roles.Admin});

            modelBuilder.Entity<CurrencyAll>()
                .HasData(new CurrencyAll {CurrencyName = "USD",InputCommision = (decimal)0.2});
        }
    }
}