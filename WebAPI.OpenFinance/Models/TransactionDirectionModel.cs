using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("transaction_direction")]
    public class TransactionDirectionModel
    {
        [Key]
        [Column("transaction_direction_id")]
        public int TransactionDirectionID { get; set; }

        [Required]
        [Column("transaction_direction_name")]
        public string TransactionDirectionName { get; set; }
    }
}
