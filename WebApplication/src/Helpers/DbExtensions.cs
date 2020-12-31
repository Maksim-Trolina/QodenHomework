using System;
using System.Collections.Generic;
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

        public static async Task<User> GetUserAsync(this Db db, Guid id)
        {
            return await db.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        /*
        public static  Account GetAccount(this Db db, Guid userId, string name, string password)
        {
            var hasher = new PasswordHasher<string>();

            return  db.Accounts.ToList().FirstOrDefault(x => x.UserId == userId &&
                                                                      x.Name == name &&
                                                                      hasher.VerifyHashedPassword(name, x.Password, 
                                                                          password) == PasswordVerificationResult.Success);
        }
        */

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

        public static async Task AddUserAsync(this Db db, string email,string accountName,string password)
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

        public static async Task AddAccountAsync(this Db db,User user,string name,string password)
        {
            var hasher = new PasswordHasher<string>();
            
            var hashPassword = hasher.HashPassword(name,password);
            
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

        private static async Task AddAccountCurrencyAsync(Db db, Account account,CurrencyInformation currencyInformation)
        {
            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                Account = account,
                CurrencyInformation = currencyInformation,
                Value = 0
            };

            await db.AccountCurrencies.AddAsync(accountCurrency);
            
        }

        private static async Task AddAccountCurrenciesAsync(Db db,Account account)
        {
            var currencyInformation = await db.CurrencyInformation.ToListAsync();

            foreach (var curInfo in currencyInformation)
            {
                await AddAccountCurrencyAsync(db, account, curInfo);
            }
        }
    }
}