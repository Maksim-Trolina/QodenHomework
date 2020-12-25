using System;
using System.Collections.Generic;

namespace WebApplication.Database.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public List<Account> Accounts { get; set; }

        public List<UserDepositCommission> UserDepositCommissions { get; set; }

        public List<UserWithdrawCommission> UserWithdrawCommissions { get; set; }

        public List<UserTransferCommission> UserTransferCommissions { get; set; }
    }
}