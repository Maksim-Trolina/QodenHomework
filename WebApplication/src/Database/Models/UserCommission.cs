using System;

namespace WebApplication.Database.Models
{
    public class UserCommission
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }
        
        public string CurrencyName { get; set; }

        public virtual CurrencyInformation CurrencyInformation { get; set; }

        public decimal? DepositCommission { get; set; }

        public decimal? WithdrawCommission { get; set; }

        public decimal? TransferCommission { get; set; }
    }
}