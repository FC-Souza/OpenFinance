using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("mutual_fund_info")]
    public class MutualFundInfoModel
    {
        /*
         * Table: mutual_fund_info
         * mfi_id (PK, int, not null)
         * mf_id (FK, int, not null)
         * connetion_id (FK, int, not null)
         * qty_shares (int, not null)
         * average_nav (decimal(15, 2), not null)
         * last_updated (datetime, not null)
         */

        [Key]
        [Column("mfi_id")]
        public int MFIID { get; init; }

        [Column("mf_id")]
        public int MFID { get; set; }
        [ForeignKey("MFID")]
        public MutualFundModel MutualFund { get; set; }

        [Column("connection_id")]
        public int ConnectionID { get; set; }
        [ForeignKey("connectionID")]
        public ConnectionsModel Connection { get; set; }

        [Column("quantity_shares")]
        public int QuantityShares { get; set; }

        [Column("average_nav")]
        public decimal AverageNAV { get; set; }

        [Column("last_updated")]
        public DateTime LastUpdated { get; set; }

    }
}
