using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Database;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("api/v1/user")]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly IDb db;

        private readonly OperationService operationService;

        public UserController(Db db, OperationService operationService)
        {
            this.db = db;

            this.operationService = operationService;
        }

        [HttpPost("account/create")]
        public async Task CreateAccountAsync(string accountName, string password)
        {
            var account = await db.GetAccountAsync(Guid.Parse(User.Identity.Name));

            var user = account.User;

            var duplicate = db.GetAccount(user, accountName, password);

            if (duplicate == null)
            {
                await db.AddAccountAsync(user, accountName, password);
            }
        }

        [HttpDelete("account/delete")]
        public async Task DeleteAccountAsync(string accountName)
        {
            var currentAccount = await db.GetAccountAsync(Guid.Parse(User.Identity.Name));

            await db.RemoveAccountAsync(accountName, currentAccount.UserId);


            if (currentAccount.Name == accountName)
            {
                await SignOutAsync();
            }
        }

        [HttpPut("deposit")]
        public async Task DepositAsync(string currencyName, decimal value)
        {
            await operationService.DepositForUserAsync(Guid.Parse(User.Identity.Name), currencyName, value);
        }

        [HttpPut("withdraw")]
        public async Task WithdrawAsync(string currencyName, decimal value)
        {
            await operationService.WithdrawForUserAsync(Guid.Parse(User.Identity.Name), currencyName, value);
        }

        [HttpPut("transfer")]
        public async Task TransferAsync(string currencyName, decimal value, Guid toAccountId)
        {
            var fromAccountId = Guid.Parse(User.Identity.Name);

            if (toAccountId != fromAccountId)
            {
                await operationService.TransferForUserAsync(fromAccountId, toAccountId, currencyName,
                    value);
            }
        }

        private async Task SignOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}