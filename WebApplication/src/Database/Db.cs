using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication.Database.Models;
using WebApplication.Helpers;

namespace WebApplication.Database
{
    public class Db : DbContext, IDb
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

        public async Task<User> GetUserAsync(string email)
        {
            return await Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Account GetAccount(User user, string name, string password)
        {
            var hasher = new PasswordHasher<string>();

            return user.Accounts.FirstOrDefault(x => x.Name == name &&
                                                     hasher.VerifyHashedPassword(name, x.Password,
                                                         password) == PasswordVerificationResult.Success);
        }

        public async Task<Account> GetAccountAsync(Guid id)
        {
            return await Accounts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AccountCurrency> GetAccountCurrencyAsync(Guid accountId,
            string currencyName)
        {
            return await AccountCurrencies.FirstOrDefaultAsync(x => x.AccountId == accountId
                                                                    && x.CurrencyName == currencyName);
        }

        public async Task<UserCommission> GetUserCommissionAsync(Guid userId, string currencyName)
        {
            return await UserCommissions.FirstOrDefaultAsync(x => x.UserId == userId
                                                                  && x.CurrencyName == currencyName);
        }

        public async Task<Operation> GetOperationAsync(Guid id)
        {
            return await Operations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddUserAsync(string email, string accountName, string password)
        {
            var user = new User
            {
                Email = email,
                Role = Role.User,
                RegistrationDate = DateTime.Now,
                Id = Guid.NewGuid()
            };

            await Users.AddAsync(user);

            await AddAccountAsync(user, accountName, password);

            await SaveChangesAsync();
        }

        public async Task AddAccountAsync(User user, string name, string password)
        {
            var hasher = new PasswordHasher<string>();

            var hashPassword = hasher.HashPassword(name, password);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                User = user,
                Name = name,
                Password = hashPassword,
                RegistrationDate = DateTime.Now
            };

            await Accounts.AddAsync(account);

            await AddAccountCurrenciesAsync(account);

            await SaveChangesAsync();
        }

        public async Task AddCurrencyAsync(Currency currency)
        {
            var duplicate = await Currencies.FirstOrDefaultAsync(x => x.Name == currency.Name);

            if (duplicate == null)
            {
                await Currencies.AddAsync(currency);

                await AddAccountCurrenciesAsync(currency);

                await SaveChangesAsync();
            }
        }

        public async Task AddOperationAsync(Operation operation)
        {
            await Operations.AddAsync(operation);

            await SaveChangesAsync();
        }

        public async Task AddOrUpdateUserCommissionAsync(string currencyName, Guid userId,
            decimal? depositRelativeCommission, decimal? withdrawRelativeCommission,
            decimal? transferRelativeCommission)
        {
            var userCommission = await UserCommissions.FirstOrDefaultAsync(x => x.UserId == userId
                && x.CurrencyName == currencyName);

            var isCreate = false;

            if (userCommission == null)
            {
                var user = await Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                {
                    return;
                }

                var currency = await GetCurrencyAsync(currencyName);

                userCommission = new UserCommission
                {
                    Id = Guid.NewGuid(),
                    User = user,
                    Currency = currency
                };

                isCreate = true;
            }

            if (depositRelativeCommission.HasValue)
            {
                userCommission.DepositRelativeCommission = depositRelativeCommission.Value;
            }

            if (withdrawRelativeCommission.HasValue)
            {
                userCommission.WithdrawRelativeCommission = withdrawRelativeCommission.Value;
            }

            if (transferRelativeCommission.HasValue)
            {
                userCommission.TransferRelativeCommission = transferRelativeCommission.Value;
            }

            if (isCreate)
            {
                await UserCommissions.AddAsync(userCommission);
            }

            await SaveChangesAsync();
        }

        public async Task UpdateCurrencyCommissionAsync(string name,
            decimal? depositRelativeCommission,
            decimal? withdrawRelativeCommission, decimal? transferRelativeCommission,
            decimal? depositAbsoluteCommission, decimal? withdrawAbsoluteCommission,
            decimal? transferAbsoluteCommission)
        {
            var currency = await Currencies.FirstOrDefaultAsync(x => x.Name == name);

            if (currency == null)
            {
                return;
            }

            if (depositRelativeCommission.HasValue)
            {
                currency.DepositRelativeCommission = depositRelativeCommission.Value;
            }

            if (withdrawRelativeCommission.HasValue)
            {
                currency.WithdrawRelativeCommission = withdrawRelativeCommission.Value;
            }

            if (transferRelativeCommission.HasValue)
            {
                currency.TransferRelativeCommission = transferRelativeCommission.Value;
            }

            if (depositAbsoluteCommission.HasValue)
            {
                currency.DepositAbsoluteCommission = depositAbsoluteCommission.Value;
            }

            if (withdrawAbsoluteCommission.HasValue)
            {
                currency.WithdrawAbsoluteCommission = withdrawAbsoluteCommission.Value;
            }

            if (transferAbsoluteCommission.HasValue)
            {
                currency.TransferAbsoluteCommission = transferAbsoluteCommission.Value;
            }

            await SaveChangesAsync();
        }

        public async Task UpdateCurrencyLimitAsync(string name, decimal? deposit, decimal? withdraw,
            decimal? transfer)
        {
            var currency = await Currencies.FirstOrDefaultAsync(x => x.Name == name);

            if (currency == null)
            {
                return;
            }

            if (deposit.HasValue)
            {
                currency.DepositLimit = deposit.Value;
            }

            if (withdraw.HasValue)
            {
                currency.WithdrawLimit = withdraw.Value;
            }

            if (transfer.HasValue)
            {
                currency.TransferLimit = transfer.Value;
            }

            await SaveChangesAsync();
        }

        public async Task RemoveCurrencyAsync(string name)
        {
            var currency = await Currencies.FirstOrDefaultAsync(x => x.Name == name);

            if (currency != null)
            {
                Currencies.Remove(currency);

                await SaveChangesAsync();
            }
        }

        public async Task RemoveUserCommissionAsync(string currencyName, Guid userId,
            TypeOperation operation)
        {
            var userCommission = await UserCommissions.FirstOrDefaultAsync(x => x.UserId == userId
                && x.CurrencyName == currencyName);

            if (userCommission != null)
            {
                switch (operation)
                {
                    case TypeOperation.Deposit:
                        userCommission.DepositRelativeCommission = null;
                        break;
                    case TypeOperation.Withdraw:
                        userCommission.WithdrawRelativeCommission = null;
                        break;
                    case TypeOperation.Transfer:
                        userCommission.TransferRelativeCommission = null;
                        break;
                }

                if (userCommission.DepositRelativeCommission == null
                    && userCommission.WithdrawRelativeCommission == null
                    && userCommission.TransferRelativeCommission == null)
                {
                    UserCommissions.Remove(userCommission);
                }

                await SaveChangesAsync();
            }
        }

        public async Task RemoveAccountAsync(string accountName, Guid userId)
        {
            var account = await Accounts.FirstOrDefaultAsync(x => x.Name == accountName
                                                                  && x.UserId == userId);

            if (account != null)
            {
                Accounts.Remove(account);

                await SaveChangesAsync();
            }
        }

        public async Task TryAddAccountCurrencyAsync(Guid accountId, string currencyName)
        {
            var accountCurrency = await GetAccountCurrencyAsync(accountId, currencyName);

            if (accountCurrency == null)
            {
                var account = await GetAccountAsync(accountId);

                var currency = await GetCurrencyAsync(currencyName);

                await AddAccountCurrencyAsync(account, currency);

                await SaveChangesAsync();
            }
        }

        private async Task AddAccountCurrenciesAsync(Account account)
        {
            var currencyInformation = await Currencies.ToListAsync();

            foreach (var curInfo in currencyInformation)
            {
                await AddAccountCurrencyAsync(account, curInfo);
            }
        }

        private async Task AddAccountCurrenciesAsync(Currency currency)
        {
            var accounts = await Accounts.ToListAsync();

            foreach (var account in accounts)
            {
                await AddAccountCurrencyAsync(account, currency);
            }
        }

        private async Task AddAccountCurrencyAsync(Account account, Currency currency)
        {
            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                Account = account,
                Currency = currency,
                Value = 0
            };

            await AccountCurrencies.AddAsync(accountCurrency);
        }

        private async Task<Currency> GetCurrencyAsync(string name)
        {
            return await Currencies.FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}