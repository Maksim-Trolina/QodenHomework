using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Database;
using WebApplication.Database.Models;
using WebApplication.Helpers;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("api/v1/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly Db db;

        private readonly OperationService operationService;

        public AdminController(Db db, OperationService operationService)
        {
            this.db = db;

            this.operationService = operationService;
        }

        [HttpPost("currency/create")]
        public async Task CreateCurrency(Currency currency)
        {
            await db.AddCurrencyAsync(currency);

            await db.SaveChangesAsync();
        }

        [HttpDelete("currency/delete")]
        public async Task DeleteCurrency(string name)
        {
            await db.RemoveCurrencyAsync(name);

            await db.SaveChangesAsync();
        }

        [HttpPut("currency/commission/update")]
        public async Task UpdateCommission(string currencyName, decimal? depositRelativeCommission,
            decimal? withdrawRelativeCommission, decimal? transferRelativeCommission,
            decimal? depositAbsoluteCommission, decimal? withdrawAbsoluteCommission,
            decimal? transferAbsoluteCommission)
        {
            await db.UpdateCurrencyCommissionAsync(currencyName, depositRelativeCommission, withdrawRelativeCommission,
                transferRelativeCommission, depositAbsoluteCommission, withdrawAbsoluteCommission,
                transferAbsoluteCommission);

            await db.SaveChangesAsync();
        }

        [HttpPut("currency/limit/update")]
        public async Task UpdateLimit(string currencyName, decimal? deposit, decimal? withdraw, decimal? transfer)
        {
            await db.UpdateCurrencyLimitAsync(currencyName, deposit, withdraw, transfer);

            await db.SaveChangesAsync();
        }

        [HttpPut("currency/commission/user/update")]
        public async Task UpdateCommissionUser(string currencyName, Guid userId, decimal? depositRelativeCommission,
            decimal? withdrawRelativeCommission,
            decimal? transferRelativeCommission)
        {
            await db.AddOrUpdateUserCommissionAsync(currencyName, userId, depositRelativeCommission,
                withdrawRelativeCommission, transferRelativeCommission);

            await db.SaveChangesAsync();
        }

        [HttpDelete("currency/commission/user/delete")]
        public async Task DeleteUserCommission(string currencyName, Guid userId, TypeOperation operation)
        {
            await db.RemoveUserCommissionAsync(currencyName, userId, operation);

            await db.SaveChangesAsync();
        }

        [HttpPut("deposit/user")]
        public async Task Deposit(Guid toAccountId, string currencyName, decimal value)
        {
            await operationService.DepositForAdminAsync(Guid.Parse(User.Identity.Name), toAccountId, currencyName,
                value);

            await db.SaveChangesAsync();
        }

        [HttpPut("withdraw/user")]
        public async Task Withdraw(Guid toAccountId, string currencyName, decimal value)
        {
            await operationService.WithdrawForAdminAsync(Guid.Parse(User.Identity.Name), toAccountId, currencyName,
                value);

            await db.SaveChangesAsync();
        }

        [HttpPut("operation/confirm")]
        public async Task ConfirmOperation(Guid operationId)
        {
            await operationService.ConfirmOperationAsync(operationId);

            await db.SaveChangesAsync();
        }
    }
}