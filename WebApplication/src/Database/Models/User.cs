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

        public List<Account> Accounts { get; set; }

        public List<UserCommission> UserCommissions { get; set; }
        
    }
}