using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication.Database;
using WebApplication.Database.Models;
using WebApplication.Helpers;

namespace WebApplication.Services
{
    public class DatabaseService
    {
        private Db db;
        
        public DatabaseService(Db db)
        {
            this.db = db;
        }

        public async Task<Account> GetAccount(string mail, string name, string password)
        {
            return await db.Accounts.FirstOrDefaultAsync(x => x.UserMail == mail &&
                                                              x.AccountName == name &&
                                                              x.Password == password);
        }

        public async Task<Account> GetAccount(string name)
        {
            return await db.Accounts.FirstOrDefaultAsync(x => x.AccountName == name);
        }

        public async Task AddAccount(string mail, string name, string password)
        {
            await db.Accounts.AddAsync(new Account
            {
                UserMail = mail,
                AccountName = name,
                Password = password,
                Role = (byte) Roles.User
            });

            await AddCurrenciesAccount(name);
        }

        public async Task Save()
        {
            await db.SaveChangesAsync();
        }

        public async Task AddCurrenciesAccount(string name)
        {
            IEnumerable<CurrencyAll> currencyAlls = db.CurrencyAlls;

            foreach (var currencyAll in currencyAlls)
            {
                await AddCurrencyAccount(name, currencyAll.CurrencyName);
            }
        }

        public async Task AddUser(string mail, string name, string password)
        {
            await AddAccount(mail, name, password);

            await AddCurrenciesUser(mail);
        }

        private async Task AddCurrencyAccount(string name,string currency)
        {
            CurrencyAccount currencyAccount = new CurrencyAccount
            {
                AccountName = name,
                Count = 0,
                CurrencyName = currency,
                Id = Guid.NewGuid()
            };

            await db.CurrencyAccounts.AddAsync(currencyAccount);
        }

        private async Task AddCurrencyUser(string mail,CurrencyAll currencyAll)
        {
            CurrencyUser currencyUser = new CurrencyUser
                {
                    CurrencyName = currencyAll.CurrencyName,
                    InputCommision = currencyAll.InputCommision,
                    OutputCommision = currencyAll.OutputCommision,
                    TransferCommision = currencyAll.TransferCommision,
                    Mail = mail,
                    Id = Guid.NewGuid(),
                    IsUniqueCommision = false
                };

            await db.CurrencyUsers.AddAsync(currencyUser);
        }

        public async Task AddCurrenciesUser(string mail)
        {
            List<CurrencyAll> currencyAlls = db.CurrencyAlls.ToList();

            foreach (var currencyAll in currencyAlls)
            {
                await AddCurrencyUser(mail, currencyAll);
            }
        }

        public async Task<string> GetMail(string name)
        {
            Account account = await db.Accounts.FirstOrDefaultAsync(x => x.AccountName == name);

            return account.UserMail;
        }

        public async Task<byte> GetRole(string name)
        {
            Account account = await db.Accounts.FirstOrDefaultAsync(x => x.AccountName == name);

            return account.Role;
        }

        public async Task<CurrencyAccount> GetCurrencyAccount(string name, string currency)
        {
            return await db.CurrencyAccounts.FirstOrDefaultAsync(x => x.AccountName == name &&
                                                               x.CurrencyName == currency);
        }

        public async Task<CurrencyAll> GetCurrencyAll(string currency)
        {
            return await db.CurrencyAlls.FirstOrDefaultAsync(x => x.CurrencyName == currency);
        }

        public async Task<CurrencyUser> GetCurrencyUser(string mail, string currency)
        {
            return await db.CurrencyUsers.FirstOrDefaultAsync(x => x.Mail == mail &&
                                                                   x.CurrencyName == currency);
        }

        public async Task DeleteAccount(string name)
        {
            DeteleCurrencyAccount(name);
            
            Account account = await db.Accounts.FirstOrDefaultAsync(u => u.AccountName == name);

            db.Accounts.Remove(account);
        }

        public async Task DeleteUser(string mail)
        {
            await DeleteAccounts(mail);
            
            DeleteCurrencyUser(mail);
        }

        private async Task DeleteAccounts(string mail)
        {
            IEnumerable<string> accountsName = db.Accounts.Where(x => x.UserMail == mail).Select(x => x.AccountName);

            foreach (var name in accountsName)
            {
                await DeleteAccount(name);
            }
        }

        private void DeleteCurrencyUser(string mail)
        {
            IEnumerable<CurrencyUser> currencyUsers = db.CurrencyUsers.Where(x => x.Mail == mail);
            
            db.CurrencyUsers.RemoveRange(currencyUsers);
        }

        private void DeteleCurrencyAccount(string name)
        {
            IEnumerable<CurrencyAccount> currencyAccounts = db.CurrencyAccounts.Where(u => u.AccountName == name);
            
            db.CurrencyAccounts.RemoveRange(currencyAccounts);
        }
    }
}