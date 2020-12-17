using System;

namespace WebApplication
{
    public class CurrencyUser
    {
        public string CurrencyName { get; set; }
        public string UserName { get; set; }
        
        public decimal Count { get; set; }
        
        public decimal InputCommision { get; set; }
        
        public decimal OutputCommision { get; set; }
        
        public decimal TransferCommision { get; set; }
    }
}