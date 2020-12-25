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

        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountCurrency> AccountCurrencies { get; set; }

        public DbSet<CurrencyInformation> CurrencyInformations { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<UserDepositCommission> UserDepositCommissions { get; set; }

        public DbSet<UserWithdrawCommission> UserWithdrawCommissions { get; set; }

        public DbSet<UserTransferCommission> UserTransferCommissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetInternalKeys(modelBuilder);
            
            CreateDefaultUsers(modelBuilder);
        }

        private void SetInternalKeys(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Account>()
                .HasKey(u => u.AccountName);

            modelBuilder.Entity<CurrencyAll>()
                .HasKey(u => u.CurrencyName);

            modelBuilder.Entity<CurrencyUser>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<CurrencyAccount>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<BigOperation>()
                .HasKey(u => u.Id);*/
        }

        private void CreateDefaultUsers(ModelBuilder modelBuilder)
        {
            User admin = new User
            {
                Id = Guid.NewGuid(),
                Email = "Admin@com",
                Role = "Admin"
            };

            Account account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = admin.Id,
                Name = "Admin",
                Password = "Admin"
            };

            admin.Accounts = new List<Account> {account};

            modelBuilder.Entity<User>()
                .HasData(admin);
        }
    }
}