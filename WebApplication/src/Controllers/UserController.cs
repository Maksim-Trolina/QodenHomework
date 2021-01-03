using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Database;
using WebApplication.Helpers;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("user")]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly Db db;

        private readonly OperationService operationService;

        public UserController(Db db,OperationService operationService)
        {
            this.db = db;

            this.operationService = operationService;
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
        public async Task Transfer(string currencyName,decimal value,Guid toAccountId)
        {
            Guid fromAccountId = Guid.Parse(User.Identity.Name);

            if (toAccountId != fromAccountId)
            {
                await operationService.TransferForUserAsync(fromAccountId, toAccountId, currencyName,
                    value);

                await db.SaveChangesAsync();
            }
        }
    }
}