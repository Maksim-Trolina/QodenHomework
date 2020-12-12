using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication.Database;
using WebApplication.Database.Models;

namespace WebApplication.Services
{
    public interface IDbService
    {
        Task<User> GetUser(string userName, string password);
        
        Task<User> AddUser(string userName,string password);

        Task<Account> AddAccount(string userId);

        Task AddMoney(string currency, decimal value, string accountId);
    }
    public class DbService : IDbService
    {
        private UserDbContext userDb;

        public DbService(UserDbContext userDb)
        {
            this.userDb = userDb;
        }

        public async Task<User> GetUser(string userName,string password)
        {
            return await userDb.Users.FirstOrDefaultAsync(x => x.UserName == userName &&
                                                               x.Password == password);
        }
        /*public async Task<int> GetLastId()
        {
            return await userDb.Users.MaxAsync(x => x.Id);
        }*/

        public async Task<User> AddUser(string userName,string password)
        {
            /*int id = await userDb.Users.MaxAsync(x => x.Id);*/
            var user = new User{Id = Guid.NewGuid(),UserName = userName,Password = password,Role = "User"};
            
            await userDb.Users.AddAsync(user);

            await userDb.SaveChangesAsync();

            return user;
        }

        public async Task<Account> AddAccount(string userId)
        {
            var account = new Account {Id = Guid.NewGuid(), UserId = Guid.Parse(userId)};

            var currencies = userDb.CurrencyNames.ToList();

            foreach (var currency in currencies)
            {
                CurrencyUser cur = new CurrencyUser
                {
                    Id = Guid.NewGuid(),
                    AccountId = account.Id,
                    Name = currency.Id,
                    Value = 0,
                    InputCommision = currency.InputCommision,
                    InputLimit = currency.InputLimit,
                    OutputCommision = currency.OutputCommision,
                    OutputLimit = currency.OutputLimit,
                    TransferCommision = currency.TransferCommision,
                    TransferLimit = currency.TransferLimit
                };

                await userDb.CurrencyUsers.AddAsync(cur);
            }

            await userDb.Accounts.AddAsync(account);

            await userDb.SaveChangesAsync();

            return account;
        }

        public async Task AddMoney(string currency, decimal value, string accountId)
        {
            Guid id = Guid.Parse(accountId);
            CurrencyUser currencyUser = await userDb.CurrencyUsers.FirstOrDefaultAsync(x => x.AccountId == id &&
                x.Name == currency);
            
            currencyUser.Value += value;

            userDb.CurrencyUsers.Update(currencyUser);

            await userDb.SaveChangesAsync();
        }
    }
}