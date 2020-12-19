using System;
using System.Collections.Generic;

namespace WebApplication.Database.Models
{
    public class Account
    {
        public string UserMail { get; set; }

        public string AccountName { get; set; }

        public string Password { get; set; }

        public byte Role { get; set; }
    }
}