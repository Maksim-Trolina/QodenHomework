using System;
using System.Collections.Generic;

namespace WebApplication.Database.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public List<Currency> Currencies { get; set; }
    }
}