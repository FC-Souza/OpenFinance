using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPI.OpenFinance.Responses
{
    public class BenchmarkIndexes
    {
        [JsonIgnore]
        public DateTime BenchmarkPeriod { get; set; }

        [NotMapped]
        [JsonPropertyName("BenchmarkPeriod")]
        public string FormattedBenchmarkPeriod => BenchmarkPeriod.ToString("MM-yyyy");

        public decimal CPI { get; set; }

        public decimal SPTSX { get; set; }

        public decimal CBPR { get; set; }
    }
}
