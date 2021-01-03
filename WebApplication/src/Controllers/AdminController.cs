using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using WebApplication.Database;
using WebApplication.Database.Models;
using WebApplication.Helpers;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly Db db;

        private readonly OperationService operationService;

        public AdminController(Db db,OperationService operationService)
        {
            this.db = db;

            this.operationService = operationService;
        }
        
        [HttpPost("add/currency")]
        public async Task AddCurrency(Currency currency)
        {
            await db.AddCurrencyAsync(currency);

            await db.SaveChangesAsync();
        }

        [HttpDelete("remove/currency")]
        public async Task RemoveCurrency(string name)
        {
            await db.RemoveCurrencyAsync(name);

            await db.SaveChangesAsync();
        }

        [HttpPut("set/commission")]
        public async Task SetCommission(string currencyName,decimal? deposit,decimal? withdraw,decimal? transfer)
        {
            await db.UpdateCurrencyCommissionAsync(currencyName, deposit, withdraw, transfer);

            await db.SaveChangesAsync();
        }

        [HttpPut("set/limit")]
        public async Task SetLimit(string currencyName, decimal? deposit, decimal? withdraw, decimal? transfer)
        {
            await db.UpdateCurrencyLimitAsync(currencyName, deposit, withdraw, transfer);

            await db.SaveChangesAsync();
        }

        [HttpPut("set/commission/user")]
        public async Task SetCommissionUser(string currencyName, Guid userId, decimal? deposit, decimal? withdraw,
            decimal? transfer)
        {
            await db.CreateOrUpdateUserCommissionAsync(currencyName, userId, deposit, withdraw, transfer);

            await db.SaveChangesAsync();
        }

        [HttpDelete("remove/commission/user")]
        public async Task RemoveUserCommission(string currencyName, Guid userId, TypeOperation operation)
        {
            await db.RemoveUserCommissionAsync(currencyName, userId, operation);

            await db.SaveChangesAsync();
        }

        [HttpPut("deposit/user")]
        public async Task Deposit(Guid toAccountId,string currencyName,decimal value)
        {
            await operationService.DepositForAdminAsync(Guid.Parse(User.Identity.Name), toAccountId, currencyName, value);

            await db.SaveChangesAsync();
        }

        [HttpPut("withdraw/user")]
        public async Task Withdraw(Guid toAccountId, string currencyName,decimal value)
        {
            await operationService.WithdrawForAdminAsync(Guid.Parse(User.Identity.Name), toAccountId, currencyName, value);

            await db.SaveChangesAsync();
        }

        [HttpPut("confirm/operation")]
        public async Task ConfirmOperation(Guid operationId)
        {
            await operationService.ConfirmOperationAsync(operationId);

            await db.SaveChangesAsync();
        }
    }
}