using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("stock")]
    public class StockModel
    {
        /*
         * Table: stock
         * stock_id (PK, int, not null)
         * product_id (FK, int, not null)
         * stock_name (varchar(20), not null)
         * ISIN (varchar(20), not null)
         * last_day_price (decimal(10,2), not null)
         * currency (varchar(10), not null)
         * exchange (varchar(20), not null)
         * last_updated (datetime, not null)
         */

        [Key]
        [Column("stock_id")]
        public int stockId { get; init; }

        [Column("product_id")]
        public int productId { get; set; }
        [ForeignKey("product_types")]
        public ProductTypesModel Product { get; set; }

        [Column("ticker")]
        public string ticker { get; set; }


        [Column("stock_name")]
        public string stockName { get; set; }

        [Column("isin")]
        public string ISIN { get; set; }

        [Column("last_day_price")]
        public decimal lastDayPrice { get; set; }

        [Column("currency")]
        public string currency { get; set; }
    
        [Column("exchange")]
        public string exchange { get; set; }

        [Column("last_updated")]
        public DateTime lastUpdated { get; set; }

    }
}
