using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("stock_info")]
    public class StockInfoModel
    {
        /*
         * table: stock_info
         * stock_id (PK, int, not null)
         * connection_id (FK, int, not null)
         * quantity (int, not null)
         * average_price (decimal(15,2), not null)
         * last_updated (datetime, not null)
         */

        [Key]
        [Column("stock_info_id")]
        public int stockInfoId { get; init; }

        [ForeignKey("stock")]
        [Column("stock_id")]
        public int stockId { get; set; }

        [Column("connection_id")]
        public int connectionId { get; set; }
        
        [Column("quantity")]
        public int quantity { get; set; }

        [Column("average_price")]
        public decimal averagePrice { get; set; }

        [Column("last_updated")]
        public DateTime lastUpdated { get; set; }

    }
}
