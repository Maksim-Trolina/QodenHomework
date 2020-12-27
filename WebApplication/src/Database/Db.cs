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

        public DbSet<CurrencyInformation> CurrencyInformation { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<UserCommission> UserCommissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetInternalKeys(modelBuilder);
            
            CreateDefaultUsers(modelBuilder);
        }

        private void SetInternalKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrencyInformation>()
                .HasKey(x => x.Name);
        }

        private void SetRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasOne(x => x.User)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserCommission>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserCommissions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AccountCurrency>()
                .HasOne(x => x.Account)
                .WithMany(x => x.AccountCurrencies)
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Operation>()
                .HasOne(x => x.FromAccount)
                .WithMany(x => x.Operations)
                .HasForeignKey(x => x.FromAccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void CreateDefaultUsers(ModelBuilder modelBuilder)
        {
            User admin = new User
            {
                Id = Guid.NewGuid(),
                Email = "Admin@com",
                Role = Role.Admin,
                RegistrationDate = DateTime.Now
            };

            Account account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = admin.Id,
                Name = "Admin",
                Password = "Admin",
                RegistrationDate = DateTime.Now
            };

            modelBuilder.Entity<User>()
                .HasData(admin);

            modelBuilder.Entity<Account>()
                .HasData(account);
        }
    }
}