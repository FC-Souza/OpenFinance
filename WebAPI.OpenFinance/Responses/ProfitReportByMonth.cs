using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPI.OpenFinance.Responses
{
    public class ProfitReportByMonth
    {
        //ChatGPT -> ReportPeriod will not be used in the response
        [JsonIgnore]
        public DateTime ReportPeriod { get; set; }

        //ChatGPT -> FormattedReportPeriod will be used in the response
        [NotMapped]
        [JsonPropertyName("ReportPeriod")]
        public string FormattedReportPeriod => ReportPeriod.ToString("MM-yyyy");

        public decimal TotalAmountInvested { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalProfitLoss { get; set; }
        public decimal TotalProfitLossPercentage { get; set; }
    }
}
