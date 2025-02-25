namespace WebAPI.OpenFinance.Models
{
    public class AssetsProductDetailsModel
    {
        public string ProductName { get; set; }
        public int NumItems { get; set; }
        public decimal ProductTotalAmount { get; set; }
        public decimal PortfolioPercentage { get; set; }
    }
}
