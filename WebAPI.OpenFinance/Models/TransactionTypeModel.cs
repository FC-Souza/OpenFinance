using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("transaction_type")]
    public class TransactionTypeModel
    {
        [Key]
        [Column("transaction_type_id")]
        public int TransactionTypeID { get; set; }

        [Required]
        [Column("transaction_type_name")]
        public string TransactionTypeName { get; set; }
    }
}
