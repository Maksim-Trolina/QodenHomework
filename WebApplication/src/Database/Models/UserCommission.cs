using System;

namespace WebApplication.Database.Models
{
    public class UserCommission
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public string CurrencyName { get; set; }

        public virtual Currency Currency { get; set; }

        public decimal? DepositRelativeCommission { get; set; }

        public decimal? WithdrawRelativeCommission { get; set; }

        public decimal? TransferRelativeCommission { get; set; }

        /*public decimal? DepositAbsoluteCommission { get; set; }

        public decimal? WithdrawAbsoluteCommission { get; set; }

        public decimal? TransferAbsoluteCommission { get; set; }*/
    }
}