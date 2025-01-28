using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("cash_info")]
    public class CashInfoModel
    {
        /*
         * Table: cash_info
         * cash_info_id (PK, int, not null)
         * cash_id (FK (cash table), int, not null)
         * connection_id (FK (connections table), int, not null)
         * amount (decimal, not null)
         * last_update (datetime, not null)
         */

        [Key]
        [Column("cash_info_id")]
        public int cashInfoId { get; set; }

        [ForeignKey("cash")]
        [Column("cash_id")]
        public int cashId { get; set; }

        [ForeignKey("connections")]
        [Column("connection_id")]
        public int connectionId { get; set; }

        [Column("amount")]
        public decimal amount { get; set; }

    }
}
