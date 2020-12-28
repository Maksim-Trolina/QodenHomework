using System.Collections.Generic;

namespace WebApplication.Database.Models
{
    public class CurrencyInformation
    {
        public string Name { get; set; }

        public decimal DepositCommission { get; set; }

        public decimal WithdrawCommission { get; set; }

        public decimal TransferCommission { get; set; }

        public decimal DepositLimit { get; set; }

        public decimal WithdrawLimit { get; set; }

        public decimal TransferLimit { get; set; }

        public List<UserCommission> UserCommissions { get; set; }

        public List<AccountCurrency> AccountCurrencies { get; set; }

        public List<Operation> Operations { get; set; }
    }
}