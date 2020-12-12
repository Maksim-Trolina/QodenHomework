using System;
using System.Collections.Generic;

namespace WebApplication.Database.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }

        /*public User User { get; set; }
        public List<CurrencyUser> Currencies { get; set; }*/
    }
}