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

        /*Task<int> GetLastId();*/

        Task<User> AddUser(string userName,string password);

        Task<Account> AddAccount(string userId);
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

            await userDb.Accounts.AddAsync(account);

            await userDb.SaveChangesAsync();

            return account;
        }
    }
}