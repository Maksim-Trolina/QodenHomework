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

        public async Task DepositForAdminAsync(Guid fromAccountId,Guid toAccountId,string currencyName ,decimal value)
        {
            var accountCurrency = await db.GetAccountCurrencyAsync(toAccountId, currencyName);

            if (accountCurrency != null)
            {
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
        }

        public async Task WithdrawForAdminAsync(Guid fromAccountId,Guid toAccountId,string currencyName ,decimal value)
        {
            var accountCurrency = await db.GetAccountCurrencyAsync(toAccountId, currencyName);

            if (accountCurrency != null)
            {
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
        }

        public async Task DepositForUserAsync(Guid accountId, string currencyName, decimal value,bool isConfirmed=false)
        {
            var accountCurrency = await db.GetAccountCurrencyAsync(accountId, currencyName);

            if (accountCurrency != null)
            {
                var account = accountCurrency.Account;

                var userCommission = await db.GetUserCommissionAsync(account.UserId, currencyName);

                var depositCommission = userCommission?.DepositCommission;

                var currency = accountCurrency.Currency;

                depositCommission ??= currency.DepositCommission;
                
                if (isConfirmed)
                {
                   Deposit(value,depositCommission,accountCurrency);
                    
                    return;
                }

                var statusOperation = StatusOperation.Confirmed;

                if (value > currency.DepositLimit)
                {
                    statusOperation = StatusOperation.Pending;
                }
                else
                {
                    Deposit(value,depositCommission,accountCurrency);
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
        }

        public async Task<bool> WithdrawForUserAsync(Guid accountId, string currencyName, decimal value,bool isConfirmed=false)
        {
            var accountCurrency = await db.GetAccountCurrencyAsync(accountId, currencyName);

            if (accountCurrency != null)
            {
                var account = accountCurrency.Account;

                var userCommission = await db.GetUserCommissionAsync(account.UserId, currencyName);

                var withdrawCommission = userCommission?.WithdrawCommission;

                var currency = accountCurrency.Currency;

                withdrawCommission ??= currency.WithdrawCommission;

                if (accountCurrency.Value >= value*(1+withdrawCommission/100))
                {
                    if (isConfirmed)
                    {
                        Withdraw(value,withdrawCommission,accountCurrency);
                        
                        return true;
                    }
                    var statusOperation = StatusOperation.Confirmed;
                    
                    if (value > currency.WithdrawLimit)
                    {
                        statusOperation = StatusOperation.Pending;
                    }
                    else
                    {
                        Withdraw(value,withdrawCommission,accountCurrency);
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
                
            }

            return false;
        }

        public async Task<bool> TransferForUserAsync(Guid fromAccountId, Guid toAccountId, string currencyName,
            decimal value, bool isConfirmed = false)
        {
            var toAccountCurrency = await db.GetAccountCurrencyAsync(toAccountId, currencyName);

            var fromAccountCurrency = await db.GetAccountCurrencyAsync(fromAccountId, currencyName);

            if (toAccountCurrency != null && fromAccountCurrency != null)
            {
                var fromAccount = fromAccountCurrency.Account;

                var userCommission = await db.GetUserCommissionAsync(fromAccount.UserId, currencyName);

                var transferCommission = userCommission?.TransferCommission;

                var currency = fromAccountCurrency.Currency;

                transferCommission ??= currency.TransferCommission;

                if (fromAccountCurrency.Value >= value*(1+transferCommission/100))
                {
                    if (isConfirmed)
                    {
                        Transfer(value,transferCommission,toAccountCurrency,fromAccountCurrency);
                        
                        return true;
                    }

                    var statusOperation = StatusOperation.Confirmed;

                    if (value > currency.TransferLimit)
                    {
                        statusOperation = StatusOperation.Pending;
                    }
                    else
                    {
                        Transfer(value,transferCommission,toAccountCurrency,fromAccountCurrency);
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
                
            }

            return false;
        }

        public async Task ConfirmOperationAsync(Guid operationId)
        {
            var operation = await db.GetOperationAsync(operationId);

            switch (operation.Type)
            {
                case TypeOperation.Deposit:
                    await DepositForUserAsync(operation.FromAccountId,operation.CurrencyName,operation.Value,true);
                    operation.UpdateStatus(StatusOperation.Confirmed);
                    break;
                case TypeOperation.Withdraw:
                    var isSuccessful = await WithdrawForUserAsync(operation.FromAccountId, operation.CurrencyName, operation.Value, true);
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

        private void Deposit(decimal value, decimal? depositCommission, AccountCurrency accountCurrency)
        {
            value = (decimal) (value * (decimal.One - depositCommission/100));

            accountCurrency.Value += value;
        }

        private void Withdraw(decimal value, decimal? withdrawCommission, AccountCurrency accountCurrency)
        {
            value = (decimal) (value * (decimal.One + withdrawCommission/100));
                        
            accountCurrency.Value -= value;
        }

        private void Transfer(decimal value, decimal? transferCommission, AccountCurrency toAccountCurrency,
            AccountCurrency fromAccountCurrency)
        {
            value = (decimal) (value * (decimal.One + transferCommission / 100));
            
            fromAccountCurrency.Value -= value;

            value /= (decimal)(decimal.One + transferCommission / 100);

            toAccountCurrency.Value += value;
        }
        
        
    }
}