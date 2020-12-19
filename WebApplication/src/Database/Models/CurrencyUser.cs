using System;

namespace WebApplication
{
    public class CurrencyUser
    {
        public Guid Id { get; set; }
        
        public string CurrencyName { get; set; }
        
        public string Mail { get; set; }

        public decimal InputCommision { get; set; }
        
        public decimal OutputCommision { get; set; }
        
        public decimal TransferCommision { get; set; }
        
        public bool IsUniqueCommision { get; set; }
    }
}