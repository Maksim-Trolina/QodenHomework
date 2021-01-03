using System;
using System.Collections.Generic;

namespace WebApplication.Database.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }

        public DateTime RegistrationDate { get; set; }

        public virtual List<AccountCurrency> AccountCurrencies { get; set; } = new List<AccountCurrency>();
        
    }
}