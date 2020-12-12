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

        Task AddMoney(string currency, decimal value, string accountId,string userId);

        public Task DeleteCurrency(string currency, string userId);

        public Task AddCurrency(string currency, string userId, decimal coast);

        public Task TransferMoney(string currency, decimal coast, string accountId,string userId);

        public Task TakeMoney(string currency, decimal coast, string accountId, string userId);

        public Task SetCommissionTransferAll(string currency, decimal commission, string userId);

        public Task SetCommissionTransferUser(string currency, decimal commision, string userId, string adminId);
    }
    public class DbService : IDbService
    {
        private Db _db;

        private IFinancialService financialService;

        public DbService(Db db,IFinancialService financialService)
        {
            this._db = db;

            this.financialService = financialService;
        }

        public async Task<User> GetUser(string userName,string password)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.UserName == userName &&
                                                               x.Password == password);
        }
        public async Task<User> AddUser(string userName,string password)
        {
            var user = new User{Id = Guid.NewGuid(),UserName = userName,Password = password,Role = "User"};
            
            await _db.Users.AddAsync(user);

            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<Account> AddAccount(string userId)
        {
            var account = new Account {Id = Guid.NewGuid(), UserId = Guid.Parse(userId)};

            var currencies = _db.CurrencyNames.ToList();

            foreach (var currency in currencies)
            {
                CurrencyUser cur = new CurrencyUser
                {
                    Id = Guid.NewGuid(),
                    AccountId = account.Id,
                    Name = currency.Id,
                    InputCommision = currency.InputCommision,
                    InputLimit = currency.InputLimit,
                    OutputCommision = currency.OutputCommision,
                    OutputLimit = currency.OutputLimit,
                    TransferCommision = currency.TransferCommision,
                    TransferLimit = currency.TransferLimit
                };

                await _db.CurrencyUsers.AddAsync(cur);
            }

            await _db.Accounts.AddAsync(account);

            await _db.SaveChangesAsync();

            return account;
        }

        public async Task AddMoney(string currency, decimal value, string accountId,string userId)
        {
            Guid id = Guid.Parse(accountId);
            Guid idUser = Guid.Parse(userId);
            Account acc = await _db.Accounts.FirstOrDefaultAsync(x => x.UserId == idUser && x.Id == id);
            if (acc == null)
            {
                return;
            }
            CurrencyUser currencyUser = await _db.CurrencyUsers.FirstOrDefaultAsync(x => x.AccountId == id &&
                x.Name == currency);
            
            currencyUser.Value += value;

            _db.CurrencyUsers.Update(currencyUser);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteCurrency(string currency, string userId)
        {
            Guid id = Guid.Parse(userId);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id &&
                                                                   x.Role == "Admin");

            if (user != null)
            {
                var cur = await _db.CurrencyNames.FirstOrDefaultAsync(x => x.Id == currency);

                if (cur != null)
                {
                    var currencies = await _db.CurrencyUsers.Where(x => x.Name == currency).ToListAsync();

                    if (currencies != null)
                    {
                        _db.CurrencyUsers.RemoveRange(currencies);
                    }

                    _db.CurrencyNames.Remove(cur);

                    await _db.SaveChangesAsync();
                }
            }
        }

        public async Task AddCurrency(string currency, string userId, decimal coast)
        {
            Guid id = Guid.Parse(userId);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id &&
                                                                x.Role == "Admin");
            if (user != null)
            {
                CurrencyAll cur = new CurrencyAll {Id = currency, Coast = coast};

                var accounts = await _db.Accounts.ToListAsync();

                if (accounts != null)
                {
                    List<CurrencyUser> currencyUsers = new List<CurrencyUser>();
                    
                    foreach (var account in accounts)
                    {
                        currencyUsers.Add(new CurrencyUser
                        {
                            Id = Guid.NewGuid(),
                            AccountId = account.Id,
                            Name = currency,
                        });
                    }

                    await _db.CurrencyUsers.AddRangeAsync(currencyUsers);
                }

                await _db.CurrencyNames.AddAsync(cur);

                await _db.SaveChangesAsync();
            }
        }

        public async Task TransferMoney(string currency, decimal coast, string accountId,string userId)
        {
            Guid id = Guid.Parse(userId);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return;
            }

            if (user.Role == "Admin")
            {
                Guid accId = Guid.Parse(accountId);
                var cur = await _db.CurrencyUsers.FirstOrDefaultAsync(x => x.AccountId == accId &&
                                                                           x.Name == currency);
                if (cur != null)
                {
                    cur.Value += coast;

                    _db.CurrencyUsers.Update(cur);

                    await _db.SaveChangesAsync();
                }
            }
            else
            {
                
            }
        }

        public async Task TakeMoney(string currency, decimal coast, string accountId, string userId)
        {
            Guid id = Guid.Parse(userId);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return;
            }

            if (user.Role == "Admin")
            {
                Guid accId = Guid.Parse(accountId);
                var cur = await _db.CurrencyUsers.FirstOrDefaultAsync(x => x.AccountId == accId &&
                                                                           x.Name == currency);
                if (cur != null)
                {
                    cur.Value -= coast;

                    _db.CurrencyUsers.Update(cur);

                    await _db.SaveChangesAsync();
                }
            }
        }

        public async Task SetCommissionTransferAll(string currency, decimal commission, string userId)
        {
            Guid id = Guid.Parse(userId);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id && x.Role == "Admin");

            if (user != null)
            {
                var cur = await _db.CurrencyNames.FirstOrDefaultAsync(x => x.Id == currency);

                cur.TransferCommision = commission;

                var currencyUsers = await _db.CurrencyUsers.Where(x => x.Name == currency).ToListAsync();

                if (currencyUsers != null)
                {
                    foreach (var curUser in currencyUsers)
                    {
                        curUser.TransferCommision = cur.TransferCommision;
                    }

                    _db.CurrencyUsers.UpdateRange(currencyUsers);
                }

                _db.CurrencyNames.Update(cur);

                await _db.SaveChangesAsync();
            }
            
        }

        public async Task SetCommissionTransferUser(string currency, decimal commision, string userId, string adminId)
        {
            Guid id = Guid.Parse(adminId);

            var admin = await _db.Users.FirstOrDefaultAsync(x => x.Id == id && x.Role == "Admin");

            if (admin != null)
            {
                Guid idUser = Guid.Parse(userId);

                var accountsId = await _db.Accounts.Where(x => x.UserId == idUser).Select(x => x.Id).ToListAsync();

                if (accountsId != null)
                {
                    foreach (var acc in accountsId)
                    {
                        var currencyUsers = await _db.CurrencyUsers.Where(x => x.AccountId == acc).ToListAsync();

                        if (currencyUsers != null)
                        {
                            foreach (var currencyUser in currencyUsers)
                            {
                                currencyUser.TransferCommision = commision;
                            }
                            
                            _db.CurrencyUsers.UpdateRange(currencyUsers);

                            await _db.SaveChangesAsync();
                        }
                    }
                }
            }
        }
        
        public async Task SetLimitTransferAll(string currency,decimal limit,string userId){
            
            Guid id = Guid.Parse(userId);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id && x.Role == "Admin");

            if (user != null)
            {
                var cur = await _db.CurrencyNames.FirstOrDefaultAsync(x => x.Id == currency);

                cur.TransferLimit = limit;

                var currencyUsers = await _db.CurrencyUsers.Where(x => x.Name == currency).ToListAsync();

                if (currencyUsers != null)
                {
                    foreach (var curUser in currencyUsers)
                    {
                        curUser.TransferLimit = cur.TransferLimit;
                    }

                    _db.CurrencyUsers.UpdateRange(currencyUsers);
                }

                _db.CurrencyNames.Update(cur);

                await _db.SaveChangesAsync();
            }}

        public async Task SetLimitTransfer(string currency, decimal limit, string userId, string adminId)
        {
            Guid id = Guid.Parse(adminId);

            var admin = await _db.Users.FirstOrDefaultAsync(x => x.Id == id && x.Role == "Admin");

            if (admin != null)
            {
                Guid idUser = Guid.Parse(userId);

                var accountsId = await _db.Accounts.Where(x => x.UserId == idUser).Select(x => x.Id).ToListAsync();

                if (accountsId != null)
                {
                    foreach (var acc in accountsId)
                    {
                        var currencyUsers = await _db.CurrencyUsers.Where(x => x.AccountId == acc).ToListAsync();

                        if (currencyUsers != null)
                        {
                            foreach (var currencyUser in currencyUsers)
                            {
                                currencyUser.TransferLimit = limit;
                            }
                            
                            _db.CurrencyUsers.UpdateRange(currencyUsers);

                            await _db.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        public async Task SetLimitInputAll(string currency, decimal limit, string userId)
        {
            Guid id = Guid.Parse(userId);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id && x.Role == "Admin");

            if (user != null)
            {
                var cur = await _db.CurrencyNames.FirstOrDefaultAsync(x => x.Id == currency);

                cur.InputLimit = limit;

                var currencyUsers = await _db.CurrencyUsers.Where(x => x.Name == currency).ToListAsync();

                if (currencyUsers != null)
                {
                    foreach (var curUser in currencyUsers)
                    {
                        curUser.InputLimit = cur.InputLimit;
                    }

                    _db.CurrencyUsers.UpdateRange(currencyUsers);
                }

                _db.CurrencyNames.Update(cur);

                await _db.SaveChangesAsync();
            }
        }

        public async Task SetLimitInput(string currency, decimal limit, string userId, string adminId)
        {
            Guid id = Guid.Parse(adminId);

            var admin = await _db.Users.FirstOrDefaultAsync(x => x.Id == id && x.Role == "Admin");

            if (admin != null)
            {
                Guid idUser = Guid.Parse(userId);

                var accountsId = await _db.Accounts.Where(x => x.UserId == idUser).Select(x => x.Id).ToListAsync();

                if (accountsId != null)
                {
                    foreach (var acc in accountsId)
                    {
                        var currencyUsers = await _db.CurrencyUsers.Where(x => x.AccountId == acc).ToListAsync();

                        if (currencyUsers != null)
                        {
                            foreach (var currencyUser in currencyUsers)
                            {
                                currencyUser.InputLimit = limit;
                            }
                            
                            _db.CurrencyUsers.UpdateRange(currencyUsers);

                            await _db.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        public async Task SetLimitOutputAll(string currency, decimal limit, string userId)
        {
            Guid id = Guid.Parse(userId);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id && x.Role == "Admin");

            if (user != null)
            {
                var cur = await _db.CurrencyNames.FirstOrDefaultAsync(x => x.Id == currency);

                cur.OutputLimit = limit;

                var currencyUsers = await _db.CurrencyUsers.Where(x => x.Name == currency).ToListAsync();

                if (currencyUsers != null)
                {
                    foreach (var curUser in currencyUsers)
                    {
                        curUser.OutputLimit = cur.OutputLimit;
                    }

                    _db.CurrencyUsers.UpdateRange(currencyUsers);
                }

                _db.CurrencyNames.Update(cur);

                await _db.SaveChangesAsync();
            }
        }

        public async Task SetLimitOutput(string currency, decimal limit, string userId, string adminId)
        {
            Guid id = Guid.Parse(adminId);

            var admin = await _db.Users.FirstOrDefaultAsync(x => x.Id == id && x.Role == "Admin");

            if (admin != null)
            {
                Guid idUser = Guid.Parse(userId);

                var accountsId = await _db.Accounts.Where(x => x.UserId == idUser).Select(x => x.Id).ToListAsync();

                if (accountsId != null)
                {
                    foreach (var acc in accountsId)
                    {
                        var currencyUsers = await _db.CurrencyUsers.Where(x => x.AccountId == acc).ToListAsync();

                        if (currencyUsers != null)
                        {
                            foreach (var currencyUser in currencyUsers)
                            {
                                currencyUser.OutputLimit = limit;
                            }
                            
                            _db.CurrencyUsers.UpdateRange(currencyUsers);

                            await _db.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        public async Task SetCommissionInputAll(string currency, decimal commission, string userId)
        {
            Guid id = Guid.Parse(userId);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id && x.Role == "Admin");

            if (user != null)
            {
                var cur = await _db.CurrencyNames.FirstOrDefaultAsync(x => x.Id == currency);

                cur.InputCommision = commission;

                var currencyUsers = await _db.CurrencyUsers.Where(x => x.Name == currency).ToListAsync();

                if (currencyUsers != null)
                {
                    foreach (var curUser in currencyUsers)
                    {
                        curUser.InputCommision = cur.InputCommision;
                    }

                    _db.CurrencyUsers.UpdateRange(currencyUsers);
                }

                _db.CurrencyNames.Update(cur);

                await _db.SaveChangesAsync();
            }
        }

        public async Task SetCommissionInput(string currency, decimal commission, string userId, string adminId)
        {
            Guid id = Guid.Parse(adminId);

            var admin = await _db.Users.FirstOrDefaultAsync(x => x.Id == id && x.Role == "Admin");

            if (admin != null)
            {
                Guid idUser = Guid.Parse(userId);

                var accountsId = await _db.Accounts.Where(x => x.UserId == idUser).Select(x => x.Id).ToListAsync();

                if (accountsId != null)
                {
                    foreach (var acc in accountsId)
                    {
                        var currencyUsers = await _db.CurrencyUsers.Where(x => x.AccountId == acc).ToListAsync();

                        if (currencyUsers != null)
                        {
                            foreach (var currencyUser in currencyUsers)
                            {
                                currencyUser.InputCommision = commission;
                            }
                            
                            _db.CurrencyUsers.UpdateRange(currencyUsers);

                            await _db.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        public async Task SetCommissionOutputAll(string currency, decimal commission, string userId)
        {
            Guid id = Guid.Parse(userId);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id && x.Role == "Admin");

            if (user != null)
            {
                var cur = await _db.CurrencyNames.FirstOrDefaultAsync(x => x.Id == currency);

                cur.OutputCommision = commission;

                var currencyUsers = await _db.CurrencyUsers.Where(x => x.Name == currency).ToListAsync();

                if (currencyUsers != null)
                {
                    foreach (var curUser in currencyUsers)
                    {
                        curUser.OutputCommision = cur.OutputCommision;
                    }

                    _db.CurrencyUsers.UpdateRange(currencyUsers);
                }

                _db.CurrencyNames.Update(cur);

                await _db.SaveChangesAsync();
            }
        }

        public async Task SetCommissionOutput(string currency, decimal commission, string userId, string adminId)
        {
            Guid id = Guid.Parse(adminId);

            var admin = await _db.Users.FirstOrDefaultAsync(x => x.Id == id && x.Role == "Admin");

            if (admin != null)
            {
                Guid idUser = Guid.Parse(userId);

                var accountsId = await _db.Accounts.Where(x => x.UserId == idUser).Select(x => x.Id).ToListAsync();

                if (accountsId != null)
                {
                    foreach (var acc in accountsId)
                    {
                        var currencyUsers = await _db.CurrencyUsers.Where(x => x.AccountId == acc).ToListAsync();

                        if (currencyUsers != null)
                        {
                            foreach (var currencyUser in currencyUsers)
                            {
                                currencyUser.OutputCommision = commission;
                            }
                            
                            _db.CurrencyUsers.UpdateRange(currencyUsers);

                            await _db.SaveChangesAsync();
                        }
                    }
                }
            }
        }
     
    }
}