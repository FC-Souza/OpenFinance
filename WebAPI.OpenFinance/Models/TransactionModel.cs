using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("transaction")]
    public class TransactionModel
    {
        [Key]
        [Column("transaction_id")]
        public int TransactionID { get; set; }

        [Required]
        [Column("statement_id")]
        public int StatementID { get; set; }
        [ForeignKey("StatementID")]
        public StatementModel Statement { get; set; }

        [Required]
        [Column("transaction_type_id")]
        public int TransactionTypeID { get; set; }
        [ForeignKey("TransactionTypeID")]
        public TransactionTypeModel TransactionType { get; set; }

        [Required]
        [Column("transaction_direction_id")]
        public int TransactionDirectionID { get; set; }
        [ForeignKey("TransactionDirectionID")]
        public TransactionDirectionModel TransactionDirection { get; set; }

        [Required]
        [Column("product_id")]
        public int ProductID { get; set; }

        //Asset name can change during his timelife, so I need to save the name of the asset instead of his ID (stockId or MFID)
        [Required]
        [Column("asset_name")]
        public string AssetName { get; set; }

        [Required]
        [Column("transaction_date", TypeName = "date")]
        public DateTime TransactionDate { get; set; }

        [Required]
        [Column("transaction_amount")]
        public decimal TransactionAmount { get; set; }

        [Required]
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }
    }
}
