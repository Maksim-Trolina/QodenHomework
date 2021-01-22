using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApplication.Database;
using WebApplication.Database.Models;
using WebApplication.Helpers;

namespace WebApplicationTest.Helpers
{
    public class FakeDb : IDb
    {
        private List<User> users;

        private List<Account> accounts;

        private List<Currency> currencies;

        private List<Operation> operations;

        private List<AccountCurrency> accountCurrencies;

        private List<UserCommission> userCommissions;

        public FakeDb()
        {
            users = new List<User>();

            accounts = new List<Account>();

            currencies = new List<Currency>();

            operations = new List<Operation>();

            accountCurrencies = new List<AccountCurrency>();

            userCommissions = new List<UserCommission>();
        }

        public void Clear()
        {
            users.Clear();

            accounts.Clear();

            currencies.Clear();

            operations.Clear();

            accountCurrencies.Clear();

            userCommissions.Clear();
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await Task.Run(() => users.FirstOrDefault(x => x.Email == email));
        }

        public Account GetAccount(User user, string name, string password)
        {
            var hasher = new PasswordHasher<string>();

            return user.Accounts.FirstOrDefault(x => x.Name == name);
        }

        public async Task<Account> GetAccountAsync(Guid id)
        {
            return await Task.Run(() => accounts.FirstOrDefault(x => x.Id == id));
        }

        public async Task<AccountCurrency> GetAccountCurrencyAsync(Guid accountId, string currencyName)
        {
            return await Task.Run(() => accountCurrencies.FirstOrDefault(x => x.AccountId == accountId
                                                                              && x.CurrencyName == currencyName));
        }

        public async Task<UserCommission> GetUserCommissionAsync(Guid userId, string currencyName)
        {
            return await Task.Run(() => userCommissions.FirstOrDefault(x => x.UserId == userId
                                                                            && x.CurrencyName == currencyName));
        }

        public async Task<Operation> GetOperationAsync(Guid id)
        {
            return await Task.Run(() => operations.FirstOrDefault(x => x.Id == id));
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

            users.Add(user);

            await AddAccountAsync(user, accountName, password);
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
                RegistrationDate = DateTime.Now,
                AccountCurrencies = new List<AccountCurrency>()
            };

            accounts.Add(account);

            user.Accounts.Add(account);

            await AddAccountCurrenciesAsync(account);
        }

        public async Task AddCurrencyAsync(Currency currency)
        {
            throw new NotImplementedException();
        }

        public void AddAccount(Account account)
        {
            accounts.Add(account);
        }

        public void AddCurrency(Currency currency)
        {
            currencies.Add(currency);
        }

        public void AddAccountCurrency(AccountCurrency accountCurrency)
        {
            accountCurrencies.Add(accountCurrency);
        }

        public void AddUserCommission(UserCommission userCommission)
        {
            userCommissions.Add(userCommission);
        }

        public async Task AddOperationAsync(Operation operation)
        {
            await Task.Run(() => operations.Add(operation));
        }

        public async Task AddOrUpdateUserCommissionAsync(string currencyName, Guid userId,
            decimal? depositRelativeCommission,
            decimal? withdrawRelativeCommission, decimal? transferRelativeCommission)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCurrencyCommissionAsync(string name, decimal? depositRelativeCommission,
            decimal? withdrawRelativeCommission,
            decimal? transferRelativeCommission, decimal? depositAbsoluteCommission,
            decimal? withdrawAbsoluteCommission,
            decimal? transferAbsoluteCommission)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCurrencyLimitAsync(string name, decimal? deposit, decimal? withdraw, decimal? transfer)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveCurrencyAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveUserCommissionAsync(string currencyName, Guid userId, TypeOperation operation)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAccountAsync(string accountName, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task TryAddAccountCurrencyAsync(Guid accountId, string currencyName)
        {
            var accountCurrency = await GetAccountCurrencyAsync(accountId, currencyName);

            if (accountCurrency == null)
            {
                var account = await GetAccountAsync(accountId);

                var currency = await GetCurrencyAsync(currencyName);

                AddAccountCurrency(account, currency);
            }
        }

        private void AddAccountCurrency(Account account, Currency currency)
        {
            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                Account = account,
                Currency = currency,
                Value = 0,
                AccountId = account.Id,
                CurrencyName = currency.Name
            };

            account.AccountCurrencies.Add(accountCurrency);

            accountCurrencies.Add(accountCurrency);
        }

        private async Task AddAccountCurrenciesAsync(Account account)
        {
            foreach (var curInfo in currencies)
            {
                await Task.Run(() => AddAccountCurrency(account, curInfo));
            }
        }

        private async Task<Currency> GetCurrencyAsync(string name)
        {
            return await Task.Run(() => currencies.FirstOrDefault(x => x.Name == name));
        }
    }
}