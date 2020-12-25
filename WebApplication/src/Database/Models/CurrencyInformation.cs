namespace WebApplication.Database.Models
{
    public class CurrencyInformation
    {
        public string Name { get; set; }

        public decimal DepositCommission { get; set; }

        public decimal WithdrawCommission { get; set; }

        public decimal TransferCommission { get; set; }

        public decimal DepositLimit { get; set; }

        public decimal WithdrawLimit { get; set; }

        public decimal TransferLimit { get; set; }
        
    }
}