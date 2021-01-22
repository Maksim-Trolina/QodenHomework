using System.Collections.Generic;

namespace WebApplication.Database.Models
{
    public class Currency
    {
        public string Name { get; set; }

        public decimal DepositRelativeCommission { get; set; }

        public decimal WithdrawRelativeCommission { get; set; }

        public decimal TransferRelativeCommission { get; set; }

        public decimal DepositAbsoluteCommission { get; set; }

        public decimal WithdrawAbsoluteCommission { get; set; }

        public decimal TransferAbsoluteCommission { get; set; }

        public decimal DepositLimit { get; set; }

        public decimal WithdrawLimit { get; set; }

        public decimal TransferLimit { get; set; }

        public virtual List<UserCommission> UserCommissions { get; set; } = new List<UserCommission>();

        public virtual List<AccountCurrency> AccountCurrencies { get; set; } = new List<AccountCurrency>();

        public virtual List<Operation> Operations { get; set; } = new List<Operation>();
    }
}