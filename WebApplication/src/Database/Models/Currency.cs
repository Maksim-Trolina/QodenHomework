using System;

namespace WebApplication
{
    public class Currency
    {
        public string Id { get; set; }
        public decimal Value { get; set; }
        public decimal InputCommision { get; set; }
        public decimal OutputCommision { get; set; }
        public decimal TransferCommision { get; set; }
        public decimal InputLimit { get; set; }
        public decimal OutputLimit { get; set; }
        public decimal TransferLimit { get; set; }
    }
}