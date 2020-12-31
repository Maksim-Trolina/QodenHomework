using System;
using System.Collections.Generic;
using WebApplication.Helpers;

namespace WebApplication.Database.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }
        
        public DateTime RegistrationDate { get; set; }

        public virtual List<Account> Accounts { get; set; } = new List<Account>();

        public virtual List<UserCommission> UserCommissions { get; set; } = new List<UserCommission>();
        
    }
}