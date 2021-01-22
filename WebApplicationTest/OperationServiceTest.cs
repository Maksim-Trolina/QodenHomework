using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using WebApplication.Database.Models;
using WebApplication.Helpers;
using WebApplication.Services;
using WebApplicationTest.Helpers;


namespace WebApplicationTest
{
    public class AuthenticateControllerTest
    {
        private FakeDb fakeDb = new FakeDb();

        [TearDown]
        public void ClearDb()
        {
            fakeDb.Clear();
        }

        [Test]
        public async Task DepositForAdminTest_DepositUser_SuccessfulDeposit()
        {
            var operationService = new OperationService(fakeDb);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var currency = new Currency
            {
                Name = "USD",
                DepositLimit = 50000,
                DepositAbsoluteCommission = 100,
                DepositRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            fakeDb.AddAccount(account);

            fakeDb.AddCurrency(currency);


            await operationService.DepositForAdminAsync(account.Id, account.Id, currency.Name, 200);


            var accountCurrency = await fakeDb.GetAccountCurrencyAsync(account.Id, currency.Name);

            Assert.AreEqual(200, accountCurrency.Value);
        }

        [Test]
        public async Task WithdrawForAdminAsyncTest_WithdrawUser_SuccessfulWithdraw()
        {
            var operationService = new OperationService(fakeDb);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var currency = new Currency
            {
                Name = "USD",
                WithdrawLimit = 50000,
                WithdrawAbsoluteCommission = 100,
                WithdrawRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                CurrencyName = currency.Name,
                Value = 200
            };

            fakeDb.AddAccount(account);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(accountCurrency);


            await operationService.WithdrawForAdminAsync(account.Id, account.Id, currency.Name, 100);


            accountCurrency = await fakeDb.GetAccountCurrencyAsync(account.Id, currency.Name);

            Assert.AreEqual(100, accountCurrency.Value);
        }

        [Test]
        public async Task DepositForUserAsyncTest_DepositWithoutUserCommission_SuccessfulDeposit()
        {
            var operationService = new OperationService(fakeDb);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var currency = new Currency
            {
                Name = "USD",
                DepositLimit = 50000,
                DepositAbsoluteCommission = 100,
                DepositRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                CurrencyName = currency.Name,
                Value = 300,
                Account = account,
                Currency = currency
            };

            fakeDb.AddAccount(account);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(accountCurrency);

            await operationService.DepositForUserAsync(account.Id, currency.Name, 1200);

            accountCurrency = await fakeDb.GetAccountCurrencyAsync(account.Id, currency.Name);

            Assert.AreEqual(1380, accountCurrency.Value);
        }

        [Test]
        public async Task DepositForUserAsyncTest_DepositWithUserCommission_SuccessfulDeposit()
        {
            var operationService = new OperationService(fakeDb);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>(),
            };

            var currency = new Currency
            {
                Name = "USD",
                DepositLimit = 50000,
                DepositAbsoluteCommission = 100,
                DepositRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                CurrencyName = currency.Name,
                Value = 300,
                Account = account,
                Currency = currency
            };

            var userCommission = new UserCommission
            {
                CurrencyName = currency.Name,
                Currency = currency,
                UserId = account.UserId,
                DepositRelativeCommission = 20
            };

            fakeDb.AddAccount(account);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(accountCurrency);

            fakeDb.AddUserCommission(userCommission);

            await operationService.DepositForUserAsync(account.Id, currency.Name, 1200);

            accountCurrency = await fakeDb.GetAccountCurrencyAsync(account.Id, currency.Name);

            Assert.AreEqual(1260, accountCurrency.Value);
        }

        [Test]
        public async Task DepositForUserAsyncTest_CommissionMoreDeposit_UnsuccessfulDeposit()
        {
            var operationService = new OperationService(fakeDb);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var currency = new Currency
            {
                Name = "USD",
                DepositLimit = 50000,
                DepositAbsoluteCommission = 100,
                DepositRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                CurrencyName = currency.Name,
                Value = 300,
                Account = account,
                Currency = currency
            };

            fakeDb.AddAccount(account);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(accountCurrency);

            await operationService.DepositForUserAsync(account.Id, currency.Name, 80);

            accountCurrency = await fakeDb.GetAccountCurrencyAsync(account.Id, currency.Name);

            Assert.AreEqual(300, accountCurrency.Value);
        }

        [Test]
        public async Task DepositForUserAsyncTest_ConfirmedDepositOperation_SuccessfulDeposit()
        {
            var operationService = new OperationService(fakeDb);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var currency = new Currency
            {
                Name = "USD",
                DepositLimit = 50000,
                DepositAbsoluteCommission = 100,
                DepositRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                CurrencyName = currency.Name,
                Value = 300,
                Account = account,
                Currency = currency
            };

            fakeDb.AddAccount(account);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(accountCurrency);

            await operationService.DepositForUserAsync(account.Id, currency.Name, 130, true);

            accountCurrency = await fakeDb.GetAccountCurrencyAsync(account.Id, currency.Name);

            Assert.AreEqual(330, accountCurrency.Value);
        }

        [Test]
        public async Task DepositForUserAsyncTest_DepositMoreLimit_SuccessfulDeposit()
        {
            var operationService = new OperationService(fakeDb);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var currency = new Currency
            {
                Name = "USD",
                DepositLimit = 50000,
                DepositAbsoluteCommission = 100,
                DepositRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                CurrencyName = currency.Name,
                Value = 300,
                Account = account,
                Currency = currency
            };

            fakeDb.AddAccount(account);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(accountCurrency);

            await operationService.DepositForUserAsync(account.Id, currency.Name, 50001);

            accountCurrency = await fakeDb.GetAccountCurrencyAsync(account.Id, currency.Name);

            Assert.AreEqual(300, accountCurrency.Value);
        }

        [Test]
        public async Task WithdrawForUserAsyncTest_WithdrawWithoutUserCommission_SuccessfulWithdraw()
        {
            var operationService = new OperationService(fakeDb);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var currency = new Currency
            {
                Name = "USD",
                WithdrawLimit = 50000,
                WithdrawAbsoluteCommission = 100,
                WithdrawRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                CurrencyName = currency.Name,
                Value = 300,
                Account = account,
                Currency = currency
            };

            fakeDb.AddAccount(account);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(accountCurrency);


            await operationService.WithdrawForUserAsync(account.Id, currency.Name, 200);


            accountCurrency = await fakeDb.GetAccountCurrencyAsync(account.Id, currency.Name);

            Assert.AreEqual(0, accountCurrency.Value);
        }

        [Test]
        public async Task WithdrawForUserAsyncTest_WithdrawMoreAccountValue_UnsuccessfulWithdraw()
        {
            var operationService = new OperationService(fakeDb);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var currency = new Currency
            {
                Name = "USD",
                WithdrawLimit = 50000,
                WithdrawAbsoluteCommission = 100,
                WithdrawRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                CurrencyName = currency.Name,
                Value = 300,
                Account = account,
                Currency = currency
            };

            fakeDb.AddAccount(account);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(accountCurrency);


            await operationService.WithdrawForUserAsync(account.Id, currency.Name, 250);


            accountCurrency = await fakeDb.GetAccountCurrencyAsync(account.Id, currency.Name);

            Assert.AreEqual(300, accountCurrency.Value);
        }

        [Test]
        public async Task TransferForUserAsyncTest_WithdrawWithUserCommission_SuccessfulTransfer()
        {
            var operationService = new OperationService(fakeDb);

            var fromAccount = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>(),
            };

            var currency = new Currency
            {
                Name = "USD",
                TransferLimit = 50000,
                TransferAbsoluteCommission = 100,
                TransferRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var fromAccountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = fromAccount.Id,
                CurrencyName = currency.Name,
                Value = 2000,
                Account = fromAccount,
                Currency = currency
            };

            var fromUserCommission = new UserCommission
            {
                CurrencyName = currency.Name,
                Currency = currency,
                UserId = fromAccount.UserId,
                TransferRelativeCommission = 20
            };

            var toAccount = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "second",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var toAccountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = toAccount.Id,
                Account = toAccount,
                Currency = currency,
                CurrencyName = currency.Name,
                Value = 200
            };

            fakeDb.AddAccount(fromAccount);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(fromAccountCurrency);

            fakeDb.AddUserCommission(fromUserCommission);

            fakeDb.AddAccount(toAccount);

            fakeDb.AddAccountCurrency(toAccountCurrency);

            await operationService.TransferForUserAsync(fromAccount.Id, toAccount.Id, currency.Name, 1000);

            fromAccountCurrency = await fakeDb.GetAccountCurrencyAsync(fromAccount.Id, currency.Name);

            toAccountCurrency = await fakeDb.GetAccountCurrencyAsync(toAccount.Id, currency.Name);

            Assert.IsTrue(fromAccountCurrency.Value == 800 && toAccountCurrency.Value == 1200);
        }

        [Test]
        public async Task TransferForUserAsyncTest_TransferMoreAccountValue_UnsuccessfulTransfer()
        {
            var operationService = new OperationService(fakeDb);

            var fromAccount = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>(),
            };

            var currency = new Currency
            {
                Name = "USD",
                TransferLimit = 50000,
                TransferAbsoluteCommission = 100,
                TransferRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var fromAccountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = fromAccount.Id,
                CurrencyName = currency.Name,
                Value = 500,
                Account = fromAccount,
                Currency = currency
            };

            var fromUserCommission = new UserCommission
            {
                CurrencyName = currency.Name,
                Currency = currency,
                UserId = fromAccount.UserId,
                TransferRelativeCommission = 20
            };

            var toAccount = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "second",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var toAccountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = toAccount.Id,
                Account = toAccount,
                Currency = currency,
                CurrencyName = currency.Name,
                Value = 200
            };

            fakeDb.AddAccount(fromAccount);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(fromAccountCurrency);

            fakeDb.AddUserCommission(fromUserCommission);

            fakeDb.AddAccount(toAccount);

            fakeDb.AddAccountCurrency(toAccountCurrency);

            await operationService.TransferForUserAsync(fromAccount.Id, toAccount.Id, currency.Name, 470);

            fromAccountCurrency = await fakeDb.GetAccountCurrencyAsync(fromAccount.Id, currency.Name);

            toAccountCurrency = await fakeDb.GetAccountCurrencyAsync(toAccount.Id, currency.Name);

            Assert.IsTrue(fromAccountCurrency.Value == 500 && toAccountCurrency.Value == 200);
        }

        [Test]
        public async Task ConfirmOperationAsync_ConfirmDeposit_ConfirmedDeposit()
        {
            var operationService = new OperationService(fakeDb);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var currency = new Currency
            {
                Name = "USD",
                DepositLimit = 50000,
                DepositAbsoluteCommission = 100,
                DepositRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                CurrencyName = currency.Name,
                Value = 300,
                Account = account,
                Currency = currency
            };

            var operation = new Operation
            {
                Id = Guid.NewGuid(),
                ToAccountId = account.Id,
                FromAccountId = account.Id,
                Currency = currency,
                CurrencyName = currency.Name,
                Type = TypeOperation.Deposit,
                Status = StatusOperation.Pending,
                Value = 500
            };

            fakeDb.AddAccount(account);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(accountCurrency);

            await fakeDb.AddOperationAsync(operation);


            await operationService.ConfirmOperationAsync(operation.Id);


            accountCurrency = await fakeDb.GetAccountCurrencyAsync(account.Id, currency.Name);

            Assert.IsTrue(operation.Status == StatusOperation.Confirmed && accountCurrency.Value == 700);
        }

        [Test]
        public async Task ConfirmOperationAsync_ConfirmWithdraw_ConfirmedWithdraw()
        {
            var operationService = new OperationService(fakeDb);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var currency = new Currency
            {
                Name = "USD",
                WithdrawLimit = 50000,
                WithdrawAbsoluteCommission = 100,
                WithdrawRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                CurrencyName = currency.Name,
                Value = 300,
                Account = account,
                Currency = currency
            };

            var operation = new Operation
            {
                Id = Guid.NewGuid(),
                ToAccountId = account.Id,
                FromAccountId = account.Id,
                Currency = currency,
                CurrencyName = currency.Name,
                Type = TypeOperation.Withdraw,
                Status = StatusOperation.Pending,
                Value = 100
            };

            fakeDb.AddAccount(account);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(accountCurrency);

            await fakeDb.AddOperationAsync(operation);


            await operationService.ConfirmOperationAsync(operation.Id);


            accountCurrency = await fakeDb.GetAccountCurrencyAsync(account.Id, currency.Name);

            Assert.IsTrue(operation.Status == StatusOperation.Confirmed && accountCurrency.Value == 100);
        }

        [Test]
        public async Task ConfirmOperationAsync_ConfirmWithdraw_NotConfirmedWithdraw()
        {
            var operationService = new OperationService(fakeDb);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var currency = new Currency
            {
                Name = "USD",
                WithdrawLimit = 50000,
                WithdrawAbsoluteCommission = 100,
                WithdrawRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var accountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                CurrencyName = currency.Name,
                Value = 300,
                Account = account,
                Currency = currency
            };

            var operation = new Operation
            {
                Id = Guid.NewGuid(),
                ToAccountId = account.Id,
                FromAccountId = account.Id,
                Currency = currency,
                CurrencyName = currency.Name,
                Type = TypeOperation.Withdraw,
                Status = StatusOperation.Pending,
                Value = 250
            };

            fakeDb.AddAccount(account);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(accountCurrency);

            await fakeDb.AddOperationAsync(operation);


            await operationService.ConfirmOperationAsync(operation.Id);


            accountCurrency = await fakeDb.GetAccountCurrencyAsync(account.Id, currency.Name);

            Assert.IsTrue(operation.Status == StatusOperation.Pending && accountCurrency.Value == 300);
        }

        [Test]
        public async Task ConfirmOperationAsync_ConfirmTransfer_ConfirmedTransfer()
        {
            var operationService = new OperationService(fakeDb);

            var fromAccount = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var currency = new Currency
            {
                Name = "USD",
                TransferLimit = 50000,
                TransferAbsoluteCommission = 100,
                TransferRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var fromAccountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = fromAccount.Id,
                CurrencyName = currency.Name,
                Value = 300,
                Account = fromAccount,
                Currency = currency
            };

            var toAccount = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "second",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var toAccountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = toAccount.Id,
                CurrencyName = currency.Name,
                Value = 200,
                Account = toAccount,
                Currency = currency
            };

            var operation = new Operation
            {
                Id = Guid.NewGuid(),
                ToAccountId = toAccount.Id,
                FromAccountId = fromAccount.Id,
                Currency = currency,
                CurrencyName = currency.Name,
                Type = TypeOperation.Transfer,
                Status = StatusOperation.Pending,
                Value = 110
            };


            fakeDb.AddAccount(fromAccount);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(fromAccountCurrency);

            fakeDb.AddAccount(toAccount);

            fakeDb.AddAccountCurrency(toAccountCurrency);

            await fakeDb.AddOperationAsync(operation);


            await operationService.ConfirmOperationAsync(operation.Id);


            fromAccountCurrency = await fakeDb.GetAccountCurrencyAsync(fromAccount.Id, currency.Name);

            Assert.IsTrue(operation.Status == StatusOperation.Confirmed && fromAccountCurrency.Value == 90 &&
                          toAccountCurrency.Value == 310);
        }

        [Test]
        public async Task ConfirmOperationAsync_ConfirmTransfer_NotConfirmedTransfer()
        {
            var operationService = new OperationService(fakeDb);

            var fromAccount = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "first",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var currency = new Currency
            {
                Name = "USD",
                TransferLimit = 50000,
                TransferAbsoluteCommission = 100,
                TransferRelativeCommission = 10,
                AccountCurrencies = new List<AccountCurrency>(),
                Operations = new List<Operation>()
            };

            var fromAccountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = fromAccount.Id,
                CurrencyName = currency.Name,
                Value = 300,
                Account = fromAccount,
                Currency = currency
            };

            var toAccount = new Account
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "second",
                AccountCurrencies = new List<AccountCurrency>()
            };

            var toAccountCurrency = new AccountCurrency
            {
                Id = Guid.NewGuid(),
                AccountId = toAccount.Id,
                CurrencyName = currency.Name,
                Value = 200,
                Account = toAccount,
                Currency = currency
            };

            var operation = new Operation
            {
                Id = Guid.NewGuid(),
                ToAccountId = toAccount.Id,
                FromAccountId = fromAccount.Id,
                Currency = currency,
                CurrencyName = currency.Name,
                Type = TypeOperation.Transfer,
                Status = StatusOperation.Pending,
                Value = 250
            };


            fakeDb.AddAccount(fromAccount);

            fakeDb.AddCurrency(currency);

            fakeDb.AddAccountCurrency(fromAccountCurrency);

            fakeDb.AddAccount(toAccount);

            fakeDb.AddAccountCurrency(toAccountCurrency);

            await fakeDb.AddOperationAsync(operation);


            await operationService.ConfirmOperationAsync(operation.Id);


            fromAccountCurrency = await fakeDb.GetAccountCurrencyAsync(fromAccount.Id, currency.Name);

            Assert.IsTrue(operation.Status == StatusOperation.Pending && fromAccountCurrency.Value == 300 &&
                          toAccountCurrency.Value == 200);
        }
    }
}