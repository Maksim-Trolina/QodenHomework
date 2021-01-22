using System;
using System.Threading.Tasks;
using WebApplication.Database;
using WebApplication.Database.Models;
using WebApplication.Helpers;

namespace WebApplication.Services
{
    public class OperationService
    {
        private readonly Db db;

        public OperationService(Db db)
        {
            this.db = db;
        }

        public async Task DepositForAdminAsync(Guid fromAccountId, Guid toAccountId, string currencyName, decimal value)
        {
            await db.TryAddAccountCurrencyAsync(toAccountId, currencyName);

            var accountCurrency = await db.GetAccountCurrencyAsync(toAccountId, currencyName);

            accountCurrency.Value += value;

            var operation = new Operation
            {
                Id = Guid.NewGuid(),
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Currency = accountCurrency.Currency,
                Type = TypeOperation.Deposit,
                Status = StatusOperation.Confirmed,
                Date = DateTime.Now,
                Value = value
            };

            await db.AddOperationAsync(operation);
        }

        public async Task WithdrawForAdminAsync(Guid fromAccountId, Guid toAccountId, string currencyName,
            decimal value)
        {
            await db.TryAddAccountCurrencyAsync(toAccountId, currencyName);

            var accountCurrency = await db.GetAccountCurrencyAsync(toAccountId, currencyName);

            accountCurrency.Value -= value;

            var operation = new Operation
            {
                Id = Guid.NewGuid(),
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Currency = accountCurrency.Currency,
                Type = TypeOperation.Withdraw,
                Status = StatusOperation.Confirmed,
                Date = DateTime.Now,
                Value = value
            };

            await db.AddOperationAsync(operation);
        }

        public async Task DepositForUserAsync(Guid accountId, string currencyName, decimal value,
            bool isConfirmed = false)
        {
            await db.TryAddAccountCurrencyAsync(accountId, currencyName);

            var accountCurrency = await db.GetAccountCurrencyAsync(accountId, currencyName);

            var account = accountCurrency.Account;

            var userCommission = await db.GetUserCommissionAsync(account.UserId, currencyName);

            var depositRelativeCommission = userCommission?.DepositRelativeCommission;

            var currency = accountCurrency.Currency;

            depositRelativeCommission ??= currency.DepositRelativeCommission;

            var depositAbsoluteCommission = currency.DepositAbsoluteCommission;

            var commission = Math.Max((decimal) (value * depositRelativeCommission / 100), depositAbsoluteCommission);

            if (value <= commission)
            {
                return;
            }

            if (isConfirmed)
            {
                Deposit(value, commission, accountCurrency);

                return;
            }

            var statusOperation = StatusOperation.Confirmed;

            if (value > currency.DepositLimit)
            {
                statusOperation = StatusOperation.Pending;
            }
            else
            {
                Deposit(value, commission, accountCurrency);
            }

            var operation = new Operation
            {
                Id = Guid.NewGuid(),
                FromAccountId = accountId,
                ToAccountId = accountId,
                Type = TypeOperation.Deposit,
                Currency = currency,
                Value = value,
                Status = statusOperation,
                Date = DateTime.Now
            };

            await db.AddOperationAsync(operation);
        }

        public async Task<bool> WithdrawForUserAsync(Guid accountId, string currencyName, decimal value,
            bool isConfirmed = false)
        {
            await db.TryAddAccountCurrencyAsync(accountId, currencyName);

            var accountCurrency = await db.GetAccountCurrencyAsync(accountId, currencyName);

            var account = accountCurrency.Account;

            var userCommission = await db.GetUserCommissionAsync(account.UserId, currencyName);

            var withdrawRelativeCommission = userCommission?.WithdrawRelativeCommission;

            var currency = accountCurrency.Currency;

            withdrawRelativeCommission ??= currency.WithdrawRelativeCommission;

            var withdrawAbsoluteCommission = currency.WithdrawAbsoluteCommission;

            var commission = Math.Max((decimal) (value * withdrawRelativeCommission / 100), withdrawAbsoluteCommission);

            if (accountCurrency.Value >= value + commission)
            {
                if (isConfirmed)
                {
                    Withdraw(value, commission, accountCurrency);

                    return true;
                }

                var statusOperation = StatusOperation.Confirmed;

                if (value > currency.WithdrawLimit)
                {
                    statusOperation = StatusOperation.Pending;
                }
                else
                {
                    Withdraw(value, commission, accountCurrency);
                }

                var operation = new Operation
                {
                    Id = Guid.NewGuid(),
                    FromAccountId = accountId,
                    ToAccountId = accountId,
                    Type = TypeOperation.Withdraw,
                    Currency = currency,
                    Value = value,
                    Status = statusOperation,
                    Date = DateTime.Now
                };

                await db.AddOperationAsync(operation);

                return true;
            }

            return false;
        }

        public async Task<bool> TransferForUserAsync(Guid fromAccountId, Guid toAccountId, string currencyName,
            decimal value, bool isConfirmed = false)
        {
            await db.TryAddAccountCurrencyAsync(toAccountId, currencyName);

            await db.TryAddAccountCurrencyAsync(fromAccountId, currencyName);

            var toAccountCurrency = await db.GetAccountCurrencyAsync(toAccountId, currencyName);

            var fromAccountCurrency = await db.GetAccountCurrencyAsync(fromAccountId, currencyName);

            var fromAccount = fromAccountCurrency.Account;

            var userCommission = await db.GetUserCommissionAsync(fromAccount.UserId, currencyName);

            var transferRelativeCommission = userCommission?.TransferRelativeCommission;

            var currency = fromAccountCurrency.Currency;

            transferRelativeCommission ??= currency.TransferRelativeCommission;

            var transferAbsoluteCommission = currency.TransferAbsoluteCommission;

            var commission = Math.Max((decimal) (value * transferRelativeCommission / 100), transferAbsoluteCommission);

            if (fromAccountCurrency.Value >= value + commission)
            {
                if (isConfirmed)
                {
                    Transfer(value, commission, toAccountCurrency,
                        fromAccountCurrency);

                    return true;
                }

                var statusOperation = StatusOperation.Confirmed;

                if (value > currency.TransferLimit)
                {
                    statusOperation = StatusOperation.Pending;
                }
                else
                {
                    Transfer(value, commission, toAccountCurrency,
                        fromAccountCurrency);
                }

                var operation = new Operation
                {
                    Id = Guid.NewGuid(),
                    FromAccountId = fromAccountId,
                    ToAccountId = toAccountId,
                    Type = TypeOperation.Transfer,
                    Currency = currency,
                    Value = value,
                    Status = statusOperation,
                    Date = DateTime.Now
                };

                await db.AddOperationAsync(operation);

                return true;
            }

            return false;
        }

        public async Task ConfirmOperationAsync(Guid operationId)
        {
            var operation = await db.GetOperationAsync(operationId);

            switch (operation.Type)
            {
                case TypeOperation.Deposit:
                    await DepositForUserAsync(operation.FromAccountId, operation.CurrencyName, operation.Value, true);
                    operation.UpdateStatus(StatusOperation.Confirmed);
                    break;
                case TypeOperation.Withdraw:
                    var isSuccessful = await WithdrawForUserAsync(operation.FromAccountId, operation.CurrencyName,
                        operation.Value, true);
                    if (isSuccessful)
                    {
                        operation.UpdateStatus(StatusOperation.Confirmed);
                    }

                    break;
                case TypeOperation.Transfer:
                    isSuccessful = await TransferForUserAsync(operation.FromAccountId, operation.ToAccountId,
                        operation.CurrencyName, operation.Value, true);
                    if (isSuccessful)
                    {
                        operation.UpdateStatus(StatusOperation.Confirmed);
                    }

                    break;
            }
        }

        private void Deposit(decimal value, decimal commission, AccountCurrency accountCurrency)
        {
            value -= commission;

            accountCurrency.Value += value;
        }

        private void Withdraw(decimal value, decimal commission, AccountCurrency accountCurrency)
        {
            value += commission;

            accountCurrency.Value -= value;
        }

        private void Transfer(decimal value, decimal commission, AccountCurrency toAccountCurrency,
            AccountCurrency fromAccountCurrency)
        {
            value += commission;

            fromAccountCurrency.Value -= value;

            value -= commission;

            toAccountCurrency.Value += value;
        }
    }
}