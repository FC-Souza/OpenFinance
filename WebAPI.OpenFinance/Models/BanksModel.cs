using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("banks")] 
    public class BanksModel
    {
        /*
         * Table: banks
         * bank_id (PK, int, not null)
         * bank_name (varchar(100), not null)
         */

        [Key]
        [Column("bank_id")]
        public int bankID { get; set; }

        [Required]
        [Column("bank_name")]
        public string bankName { get; set; }
    }
}
