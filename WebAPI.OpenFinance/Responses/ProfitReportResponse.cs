namespace WebAPI.OpenFinance.Responses
{
    public class ProfitReportResponse
    {
        public int ClientID { get; set; }

        public List<ProfitReportByMonth> ProfitReportByMonth { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
