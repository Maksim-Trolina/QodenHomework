using System;
using System.Threading.Tasks;
using WebApplication.Database.Models;
using WebApplication.Helpers;

namespace WebApplication.Database
{
    public interface IDb
    {
        public Task<User> GetUserAsync(string email);
        public Account GetAccount(User user, string name, string password);

        public Task<Account> GetAccountAsync(Guid id);

        public Task<AccountCurrency> GetAccountCurrencyAsync(Guid accountId,
            string currencyName);

        public Task<UserCommission> GetUserCommissionAsync(Guid userId, string currencyName);

        public Task<Operation> GetOperationAsync(Guid id);

        public Task AddUserAsync(string email, string accountName, string password);

        public Task AddAccountAsync(User user, string name, string password);

        public Task AddCurrencyAsync(Currency currency);

        public Task AddOperationAsync(Operation operation);

        public Task AddOrUpdateUserCommissionAsync(string currencyName, Guid userId,
            decimal? depositRelativeCommission, decimal? withdrawRelativeCommission,
            decimal? transferRelativeCommission);

        public Task UpdateCurrencyCommissionAsync(string name,
            decimal? depositRelativeCommission,
            decimal? withdrawRelativeCommission, decimal? transferRelativeCommission,
            decimal? depositAbsoluteCommission, decimal? withdrawAbsoluteCommission,
            decimal? transferAbsoluteCommission);

        public Task UpdateCurrencyLimitAsync(string name, decimal? deposit, decimal? withdraw,
            decimal? transfer);

        public Task RemoveCurrencyAsync(string name);

        public Task RemoveUserCommissionAsync(string currencyName, Guid userId,
            TypeOperation operation);

        public Task RemoveAccountAsync(string accountName, Guid userId);

        public Task TryAddAccountCurrencyAsync(Guid accountId, string currencyName);
    }
}