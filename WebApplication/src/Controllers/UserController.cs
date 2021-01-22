using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Database;
using WebApplication.Helpers;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("api/v1/user")]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly Db db;

        private readonly OperationService operationService;

        public UserController(Db db, OperationService operationService)
        {
            this.db = db;

            this.operationService = operationService;
        }

        [HttpPost("account/create")]
        public async Task CreateAccount(string accountName, string password)
        {
            var account = await db.GetAccountAsync(Guid.Parse(User.Identity.Name));

            var user = account.User;

            var duplicate = db.GetAccount(user, accountName, password);

            if (duplicate == null)
            {
                await db.AddAccountAsync(user, accountName, password);

                await db.SaveChangesAsync();
            }
        }

        [HttpDelete("account/delete")]
        public async Task DeleteAccount(string accountName)
        {
            var currentAccount = await db.GetAccountAsync(Guid.Parse(User.Identity.Name));

            await db.RemoveAccountAsync(accountName, currentAccount.UserId);

            await db.SaveChangesAsync();

            if (currentAccount.Name == accountName)
            {
                await SignOut();
            }
        }

        [HttpPut("deposit")]
        public async Task Deposit(string currencyName, decimal value)
        {
            await operationService.DepositForUserAsync(Guid.Parse(User.Identity.Name), currencyName, value);

            await db.SaveChangesAsync();
        }

        [HttpPut("withdraw")]
        public async Task Withdraw(string currencyName, decimal value)
        {
            await operationService.WithdrawForUserAsync(Guid.Parse(User.Identity.Name), currencyName, value);

            await db.SaveChangesAsync();
        }

        [HttpPut("transfer")]
        public async Task Transfer(string currencyName, decimal value, Guid toAccountId)
        {
            Guid fromAccountId = Guid.Parse(User.Identity.Name);

            if (toAccountId != fromAccountId)
            {
                await operationService.TransferForUserAsync(fromAccountId, toAccountId, currencyName,
                    value);

                await db.SaveChangesAsync();
            }
        }

        private async Task SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}