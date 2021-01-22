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
        private readonly IDb db;

        private readonly OperationService operationService;

        public AdminController(Db db, OperationService operationService)
        {
            this.db = db;

            this.operationService = operationService;
        }

        [HttpPost("currency/create")]
        public async Task CreateCurrencyAsync(Currency currency)
        {
            await db.AddCurrencyAsync(currency);
        }

        [HttpDelete("currency/delete")]
        public async Task DeleteCurrencyAsync(string name)
        {
            await db.RemoveCurrencyAsync(name);
        }

        [HttpPut("currency/commission/update")]
        public async Task UpdateCommissionAsync(string currencyName, decimal? depositRelativeCommission,
            decimal? withdrawRelativeCommission, decimal? transferRelativeCommission,
            decimal? depositAbsoluteCommission, decimal? withdrawAbsoluteCommission,
            decimal? transferAbsoluteCommission)
        {
            await db.UpdateCurrencyCommissionAsync(currencyName, depositRelativeCommission, withdrawRelativeCommission,
                transferRelativeCommission, depositAbsoluteCommission, withdrawAbsoluteCommission,
                transferAbsoluteCommission);
        }

        [HttpPut("currency/limit/update")]
        public async Task UpdateLimitAsync(string currencyName, decimal? deposit, decimal? withdraw, decimal? transfer)
        {
            await db.UpdateCurrencyLimitAsync(currencyName, deposit, withdraw, transfer);
        }

        [HttpPut("currency/commission/user/update")]
        public async Task UpdateCommissionUserAsync(string currencyName, Guid userId,
            decimal? depositRelativeCommission,
            decimal? withdrawRelativeCommission,
            decimal? transferRelativeCommission)
        {
            await db.AddOrUpdateUserCommissionAsync(currencyName, userId, depositRelativeCommission,
                withdrawRelativeCommission, transferRelativeCommission);
        }

        [HttpDelete("currency/commission/user/delete")]
        public async Task DeleteUserCommissionAsync(string currencyName, Guid userId, TypeOperation operation)
        {
            await db.RemoveUserCommissionAsync(currencyName, userId, operation);
        }

        [HttpPut("deposit/user")]
        public async Task DepositAsync(Guid toAccountId, string currencyName, decimal value)
        {
            await operationService.DepositForAdminAsync(Guid.Parse(User.Identity.Name), toAccountId, currencyName,
                value);
        }

        [HttpPut("withdraw/user")]
        public async Task WithdrawAsync(Guid toAccountId, string currencyName, decimal value)
        {
            await operationService.WithdrawForAdminAsync(Guid.Parse(User.Identity.Name), toAccountId, currencyName,
                value);
        }

        [HttpPut("operation/confirm")]
        public async Task ConfirmOperationAsync(Guid operationId)
        {
            await operationService.ConfirmOperationAsync(operationId);
        }
    }
}