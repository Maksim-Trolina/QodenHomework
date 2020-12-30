using System;
using WebApplication.Helpers;

namespace WebApplication.Database.Models
{
    public class Operation 
    {
        public Guid Id { get; set; }

        public Guid ToAccountId { get; set; }

        public Account ToAccount { get; set; }

        public Guid FromAccountId { get; set; }
        
        public Account FromAccount { get; set; }

        public TypeOperation Type { get; set; }
        
        public string CurrencyName { get; set; }

        public CurrencyInformation CurrencyInformation { get; set; }

        public decimal Value { get; set; }

        public StatusOperation Status { get; set; }

        public DateTime Date { get; set; }
    }
}