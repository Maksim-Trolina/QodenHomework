using System;
using System.Collections.Generic;

namespace WebApplication.Database.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        /*public List<Account> Accounts { get; set; }*/

        /*public List<CurrencyUser> Currencies { get; set; }*/
    }
}