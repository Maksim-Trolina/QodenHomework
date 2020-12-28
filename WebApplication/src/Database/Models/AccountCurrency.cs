using System;

namespace WebApplication.Database.Models
{
    public class AccountCurrency
    {
        public Guid Id { get; set; }
        
        public Guid AccountId { get; set; }

        public Account Account { get; set; }

        public string CurrencyName { get; set; }

        public CurrencyInformation CurrencyInformation { get; set; }

        public decimal Value { get; set; }
    }
}