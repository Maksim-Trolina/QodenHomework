using System;

namespace WebApplication.Database.Models
{
    public class BigOperation
    {
        public Guid Id { get; set; }
        
        public string FromAccountName { get; set; }

        public string ToAccountName { get; set; }

        public decimal Value { get; set; }

        public byte TypeOperation { get; set; }
    }
}