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

        public virtual List<UserCommission> UserCommissions { get; set; } = new List<UserCommission>();

        public virtual List<AccountCurrency> AccountCurrencies { get; set; } = new List<AccountCurrency>();

        public virtual List<Operation> Operations { get; set; } = new List<Operation>();
    }
}