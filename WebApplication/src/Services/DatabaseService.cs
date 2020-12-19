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

        private async Task AddCurrenciesAccount(string name)
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

        private async Task AddCurrenciesUser(string mail)
        {
            List<CurrencyAll> currencyAlls = db.CurrencyAlls.ToList();

            foreach (var currencyAll in currencyAlls)
            {
                await AddCurrencyUser(mail, currencyAll);
            }
        }

        public async Task AddCurrencyAll(string currency)
        {
            CurrencyAll currencyAll = new CurrencyAll
            {
                CurrencyName = currency
            };
            
            await db.CurrencyAlls.AddAsync(currencyAll);

            IEnumerable<string> mails = db.CurrencyUsers.Select(x => x.Mail).Distinct();

            foreach (var email in mails)
            {
                await AddCurrencyUser(email, currencyAll);
            }

            IEnumerable<CurrencyAccount> currencyAccounts = db.CurrencyAccounts;

            foreach (var currencyAccount in currencyAccounts)
            {
                await AddCurrencyAccount(currencyAccount.AccountName, currency);
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
            DeleteCurrencyAccount(name);
            
            Account account = await db.Accounts.FirstOrDefaultAsync(u => u.AccountName == name);

            db.Accounts.Remove(account);
        }

        public async Task DeleteUser(string mail)
        {
            await DeleteAccounts(mail);
            
            DeleteCurrencyUser(mail);
        }

        public async Task DeleteCurrency(string currency)
        {
            IEnumerable<CurrencyUser> currencyUsers = db.CurrencyUsers.Where(x => x.CurrencyName == currency);

            IEnumerable<CurrencyAccount> currencyAccounts = db.CurrencyAccounts.Where(x => x.CurrencyName == currency);

            CurrencyAll currencyAll = await db.CurrencyAlls.FirstOrDefaultAsync(x => x.CurrencyName == currency);

            db.CurrencyUsers.RemoveRange(currencyUsers);
            
            db.CurrencyAccounts.RemoveRange(currencyAccounts);

            db.CurrencyAlls.Remove(currencyAll);
        }

        public async Task ChangeCurrencyAllOption(string currency, decimal value,CurrencyAllOptions currencyAllOptions)
        {
            CurrencyAll currencyAll = await db.CurrencyAlls.FirstOrDefaultAsync(x => x.CurrencyName == currency);
            IEnumerable<CurrencyUser> currencyUsers = db.CurrencyUsers.Where(x => x.CurrencyName == currency &&
                !x.IsUniqueCommision);
            
            switch (currencyAllOptions)
            {
                case CurrencyAllOptions.InputCommission:
                    currencyAll.InputCommision = value;
                    foreach (var currencyUser in  currencyUsers)
                    {
                        currencyUser.InputCommision = value;
                    }
                    break;
                case CurrencyAllOptions.InputLimit:
                    currencyAll.InputLimit = value;
                    break;
                case CurrencyAllOptions.MinInput:
                    currencyAll.MinInput = value;
                    break;
                case CurrencyAllOptions.OutputCommission:
                    currencyAll.OutputCommision = value;
                    foreach (var currencyUser in currencyUsers)
                    {
                        currencyUser.OutputCommision = value;
                    }
                    break;
                case CurrencyAllOptions.OutputLimit:
                    currencyAll.OutputLimit = value;
                    break;
                case CurrencyAllOptions.MinOutput:
                    currencyAll.MinOutput = value;
                    break;
                case CurrencyAllOptions.TransferCommission:
                    currencyAll.TransferCommision = value;
                    foreach (var currencyUser in currencyUsers)
                    {
                        currencyUser.TransferCommision = value;
                    }
                    break;
                case CurrencyAllOptions.TransferLimit:
                    currencyAll.TransferLimit = value;
                    break;
                case CurrencyAllOptions.MinTransfer:
                    currencyAll.MinTransfer = value;
                    break;
            }
        }

        public async Task ChangeCurrencyAllOption(string mail,string currency, decimal value, CurrencyAllOptions currencyAllOptions)
        {
            CurrencyUser currencyUser = await db.CurrencyUsers.FirstOrDefaultAsync(x => x.Mail == mail &&
                                                                                    x.CurrencyName == currency);
            currencyUser.IsUniqueCommision = true;

            switch (currencyAllOptions)
            {
                case CurrencyAllOptions.InputCommissionUser:
                    currencyUser.InputCommision = value;
                    break;
                case CurrencyAllOptions.OutputCommissionUser:
                    currencyUser.OutputCommision = value;
                    break;
                case CurrencyAllOptions.TransferCommissionUser:
                    currencyUser.TransferCommision = value;
                    break;
            }
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

        private void DeleteCurrencyAccount(string name)
        {
            IEnumerable<CurrencyAccount> currencyAccounts = db.CurrencyAccounts.Where(u => u.AccountName == name);
            
            db.CurrencyAccounts.RemoveRange(currencyAccounts);
        }
    }
}