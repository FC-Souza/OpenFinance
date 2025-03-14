using System.Globalization;

namespace WebAPI.OpenFinance.Responses
{
    public class ProductDetails
    {
        public string ProductName { get; set; }
        public decimal ProdTotal { get; set; }
        public decimal PortfolioPercentage { get; set; }
    }
}
