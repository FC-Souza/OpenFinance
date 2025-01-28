using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("cash")]
    public class CashModel
    {
        /*
         * Table: cash
         * cash_id (PK, int, not null)
         * cash_description (varchar(100), not null)
         * last_updated (datetime, not null)
         */
        [Key]
        [Column("cash_id")]
        public int cashId { get; set; }
        
        [ForeignKey("product_types")]
        [Column("product_id")]
        public int productId { get; set; }

        [Required]
        [Column("cash_description")]
        public string cashDescription { get; set; }


        //I need to create an automatic date time for the last updated column
        //Probably need to use OnModelCreating in the context class
        //[Required]
        //[Column("last_updated")]
        //public DateTime lastUpdated { get; set; }
    }
}
