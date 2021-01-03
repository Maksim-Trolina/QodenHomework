using System;

namespace WebApplication.Database.Models
{
    public class AccountCurrency
    {
        public Guid Id { get; set; }
        
        public Guid AccountId { get; set; }

        public virtual Account Account { get; set; }
        
        public string CurrencyName { get; set; }

        public virtual Currency Currency { get; set; }

        public decimal Value { get; set; }
    }
}