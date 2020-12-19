using System;

namespace WebApplication.Database.Models
{
    public class CurrencyAccount
    {
        public Guid Id { get; set; }
        
        public string CurrencyName { get; set; }

        public decimal Count { get; set; }

        public string AccountName { get; set; }
    }
}