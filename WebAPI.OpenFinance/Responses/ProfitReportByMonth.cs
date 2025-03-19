namespace WebAPI.OpenFinance.Responses
{
    public class ProfitReportByMonth
    {
        public DateTime ReportPeriod { get; set; }
        public decimal TotalAmountInvested { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalProfitLoss { get; set; }
        public decimal TotalProfitLossPercentage { get; set; }
    }
}
