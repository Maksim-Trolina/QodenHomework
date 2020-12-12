namespace WebApplication.Database.Models
{
    public class CurrencyAll
    {
        public string Id { get; set; }
        public decimal Coast { get; set; }
        public decimal InputCommision { get; set; }
        public decimal OutputCommision { get; set; }
        public decimal TransferCommision { get; set; }
        public decimal InputLimit { get; set; }
        public decimal OutputLimit { get; set; }
        public decimal TransferLimit { get; set; }
    }
}