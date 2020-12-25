using System;

namespace WebApplication.Database.Models
{
    public class Operation
    {
        public Guid Id { get; set; }

        public Guid ToAccount { get; set; }

        public Guid FromAccount { get; set; }

        public byte Type { get; set; }

        public string CurrencyName { get; set; }

        public decimal Value { get; set; }

        public bool IsPending { get; set; }

        public DateTime Date { get; set; }
    }
}