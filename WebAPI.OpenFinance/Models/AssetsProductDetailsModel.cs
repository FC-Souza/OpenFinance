namespace WebAPI.OpenFinance.Models
{
    public class AssetsProductDetailsModel
    {
        public string ProductName { get; set; }
        public int NumItems { get; set; }
        public decimal ProductTotalAmount { get; set; }
        public decimal PortfolioPercentage { get; set; }
        public decimal ProductTotalAmountInvested { get; set; }
        public decimal ProductTotalProfitLoss { get; set; }
        public decimal ProductTotalProfitLossPercentage { get; set; }
        public List<AssetsProductItemModel> Items { get; set; }
    }
}
