using System;

namespace WebApplication.Database.Models
{
    public class UserTransferCommission
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        
        public string CurrencyName { get; set; }

        public decimal Commission { get; set; }
    }
}