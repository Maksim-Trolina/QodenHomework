using System;
using WebApplication.Helpers;

namespace WebApplication.Database.Models
{
    public class Operation 
    {
        public Guid Id { get; set; }

        public Guid ToAccountId { get; set; }

        public Guid FromAccountId { get; set; }

        public TypeOperation Type { get; set; }
        
        public string CurrencyName { get; set; }

        public virtual Currency Currency { get; set; }

        public decimal Value { get; set; }

        public StatusOperation Status { get; set; }

        public DateTime Date { get; set; }
    }
}