using System;
using Microsoft.AspNetCore.Identity;
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

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<UserCommission> UserCommissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetSnakeCase(modelBuilder);

            SetInternalKeys(modelBuilder);

            SetRelations(modelBuilder);

            CreateDefaultCurrencies(modelBuilder);

            CreateDefaultUsers(modelBuilder);
        }

        private void SetInternalKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>()
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

            modelBuilder.Entity<AccountCurrency>()
                .HasOne(x => x.Currency)
                .WithMany(x => x.AccountCurrencies)
                .HasForeignKey(x => x.CurrencyName)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserCommission>()
                .HasOne(x => x.Currency)
                .WithMany(x => x.UserCommissions)
                .HasForeignKey(x => x.CurrencyName)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Operation>()
                .HasOne(x => x.Currency)
                .WithMany(x => x.Operations)
                .HasForeignKey(x => x.CurrencyName)
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

            modelBuilder.Entity<User>()
                .HasData(admin);

            var hasher = new PasswordHasher<string>();

            Account account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = admin.Id,
                Name = "Admin",
                Password = hasher.HashPassword("Admin", "Admin"),
                RegistrationDate = DateTime.Now
            };


            modelBuilder.Entity<Account>()
                .HasData(account);
        }

        private void CreateDefaultCurrencies(ModelBuilder modelBuilder)
        {
            Currency usd = new Currency
            {
                Name = "USD",
                DepositRelativeCommission = 10,
                WithdrawRelativeCommission = 10,
                TransferRelativeCommission = 10,
                DepositAbsoluteCommission = 100,
                WithdrawAbsoluteCommission = 100,
                TransferAbsoluteCommission = 100,
                DepositLimit = 1000,
                WithdrawLimit = 1000,
                TransferLimit = 1000
            };

            modelBuilder.Entity<Currency>()
                .HasData(usd);
        }

        private void SetSnakeCase(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.SetTableName(entity.GetTableName().ToSnakeCase());

                // Replace column names
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToSnakeCase());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToSnakeCase());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.Name.ToSnakeCase());
                }
            }
        }
        
        
    }
}