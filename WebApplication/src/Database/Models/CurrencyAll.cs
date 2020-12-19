namespace WebApplication.Database.Models
{
    public class CurrencyAll
    {
        public string CurrencyName { get; set; }

        public decimal InputCommision { get; set; }
        
        public decimal OutputCommision { get; set; }
        
        public decimal TransferCommision { get; set; }
        
        public decimal InputLimit { get; set; }
        
        public decimal OutputLimit { get; set; }
        
        public decimal TransferLimit { get; set; }

        public decimal MinInput { get; set; }

        public decimal MinOutput { get; set; }

        public decimal MinTransfer { get; set; }
    }
}