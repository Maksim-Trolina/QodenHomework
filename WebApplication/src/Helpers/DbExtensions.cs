using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication.Database;
using WebApplication.Database.Models;

namespace WebApplication.Helpers
{
    public static class DbExtensions
    {
        public static async Task<User> GetUserAsync(this Db db, string email)
        {
            return await db.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public static Account GetAccount(this Db db, User user, string name, string password)
        {
            var hasher = new PasswordHasher<string>();

            return user.Accounts.FirstOrDefault(x => x.Name == name &&
                                                     hasher.VerifyHashedPassword(name, x.Password,
                                                         password) == PasswordVerificationResult.Success);
        }

        public static async Task<Account> GetAccountAsync(this Db db, Guid id)
        {
            return await db.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public static async Task<AccountCurrency> GetAccountCurrencyAsync(this Db db, Guid accountId,
            string currencyName)
        {
            return await db.AccountCurrencies.FirstOrDefaultAsync(x => x.AccountId == accountId
                                                                       && x.CurrencyName == currencyName);
        }

        public static async Task<UserCommission> GetUserCommissionAsync(this Db db, Guid userId, string currencyName)
        {
            return await db.UserCommissions.FirstOrDefaultAsync(x => x.UserId == userId
                                                                     && x.CurrencyName == currencyName);
        }

        public static async Task<Operation> GetOperationAsync(this Db db, Guid id)
        {
            return await db.Operations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public static async Task AddUserAsync(this Db db, string email, string accountName, string password)
        {
            var user = new User
            {
                Email = email,
                Role = Role.User,
                RegistrationDate = DateTime.Now,
                Id = Guid.NewGuid()
            };

            await db.Users.AddAsync(user);

            await AddAccountAsync(db, user, accountName, password);
        }

        public static async Task AddAccountAsync(this Db db, User user, string name, string password)
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

            await db.Accounts.AddAsync(account);

            await AddAccountCurrenciesAsync(db, account);
        }

        public static async Task AddCurrencyAsync(this Db db, Currency currency)
        {
            var duplicate = await db.Currencies.FirstOrDefaultAsync(x => x.Name == currency.Name);

            if (duplicate == null)
            {
                await db.Currencies.AddAsync(currency);

                await AddAccountCurrenciesAsync(db, currency);
            }
        }

        public static async Task AddOperationAsync(this Db db, Operation operation)
        {
            await db.Operations.AddAsync(operation);
        }

        public static async Task AddOrUpdateUserCommissionAsync(this Db db, string currencyName, Guid userId,
            decimal? depositRelativeCommission, decimal? withdrawRelativeCommission,
            decimal? transferRelativeCommission)
        {
            var userCommission = await db.UserCommissions.FirstOrDefaultAsync(x => x.UserId == userId
                && x.CurrencyName == currencyName);

            var isCreate = false;

            if (userCommission == null)
            {
                var user = await db.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                {
                    return;
                }

                var currency = await db.GetCurrencyAsync(currencyName);

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
                await db.UserCommissions.AddAsync(userCommission);
            }
        }

        public static async Task UpdateCurrencyCommissionAsync(this Db db, string name,
            decimal? depositRelativeCommission,
            decimal? withdrawRelativeCommission, decimal? transferRelativeCommission,
            decimal? depositAbsoluteCommission, decimal? withdrawAbsoluteCommission,
            decimal? transferAbsoluteCommission)
        {
            var currency = await db.Currencies.FirstOrDefaultAsync(x => x.Name == name);

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
        }

        public static async Task UpdateCurrencyLimitAsync(this Db db, string name, decimal? deposit, decimal? withdraw,
            decimal? transfer)
        {
            var currency = await db.Currencies.FirstOrDefaultAsync(x => x.Name == name);

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
        }

        public static async Task RemoveCurrencyAsync(this Db db, string name)
        {
            var currency = await db.Currencies.FirstOrDefaultAsync(x => x.Name == name);

            if (currency != null)
            {
                db.Currencies.Remove(currency);
            }
        }

        public static async Task RemoveUserCommissionAsync(this Db db, string currencyName, Guid userId,
            TypeOperation operation)
        {
            var userCommission = await db.UserCommissions.FirstOrDefaultAsync(x => x.UserId == userId
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
                    db.UserCommissions.Remove(userCommission);
                }
            }
        }

        public static async Task RemoveAccountAsync(this Db db, string accountName, Guid userId)
        {
            var account = await db.Accounts.FirstOrDefaultAsync(x => x.Name == accountName
                                                                     && x.UserId == userId);

            if (account != null)
            {
                db.Accounts.Remove(account);
            }
        }

        public static async Task TryAddAccountCurrencyAsync(this Db db, Guid accountId, string currencyName)
        {
            var accountCurrency = await db.GetAccountCurrencyAsync(accountId, currencyName);

            if (accountCurrency == null)
            {
                var account = await db.GetAccountAsync(accountId);

                var currency = await db.GetCurrencyAsync(currencyName);

                await AddAccountCurrencyAsync(db, account, currency);
            }
        }

        private static async Task AddAccountCurrenciesAsync(Db db, Account account)
        {
            var currencyInformation = await db.Currencies.ToListAsync();

            foreach (var curInfo in currencyInformation)
            {
                await AddAccountCurrencyAsync(db, account, curInfo);
            }
        }

        private static async Task AddAccountCurrenciesAsync(Db db, Currency currency)
        {
            var accounts = await db.Accounts.ToListAsync();

            foreach (var account in accounts)
            {
                await AddAccountCurrencyAsync(db, account, currency);
            }
        }

        private static async Task AddAccountCurrencyAsync(Db db, Account account, Currency currency)
        {
            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                Account = account,
                Currency = currency,
                Value = 0
            };

            await db.AccountCurrencies.AddAsync(accountCurrency);
        }

        private static async Task<Currency> GetCurrencyAsync(this Db db, string name)
        {
            return await db.Currencies.FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}