namespace WebAPI.OpenFinance.Models
{
    public class AssetsProductItemModel
    {
        public string ItemName { get; set; }
        public decimal ItemAmount { get; set; }
        public decimal ItemProfitLoss { get; set; }
        public decimal PortfolioPercentage { get; set; }
    }
}
