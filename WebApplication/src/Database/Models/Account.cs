using System;
using System.Collections.Generic;

namespace WebApplication.Database.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }

        public List<AccountCurrency> AccountCurrencies { get; set; }

        public List<Operation> Operations { get; set; }
    }
}