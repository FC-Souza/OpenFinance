using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("profit_report")]
    public class ProfitReportModel
    {
        [Key]
        [Column("report_id")]
        public int ReportID { get; set; }

        [Column("connection_id")]
        public int connectionId { get; set; }
        [ForeignKey("connectionId")]
        public ConnectionsModel Connection { get; set; }

        [Column("product_id")]
        public int productId { get; set; }
        [ForeignKey("product_types")]
        public ProductTypesModel Product { get; set; }

        [Column("total_amount_invested")]
        public decimal TotalAmountInvested { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("total_profit_loss")]
        public decimal TotalProfit { get; set; }
        
        [Column("total_profit_loss_percentage", TypeName = "decimal(5,2)")]
        public decimal TotalProfitPercentage { get; set; }

        [Column("report_period", TypeName = "date")]
        public DateTime ReportPeriod { get; set; }

        [Column("report_date")]
        public DateTime ReportDate { get; set; }

    }
}
