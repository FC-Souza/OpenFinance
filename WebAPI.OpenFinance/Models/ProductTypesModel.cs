using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("product_types")]
    public class ProductTypesModel
    {
        /*
         * Table: product_types
         * product_id (PK, int, not null)
         * product_type (varchar(50), not null)
         * product_description (varchar(100), not null)
         */

        [Key]
        [Column("product_id")]
        public int productId { get; init; }

        [Required]
        [Column("product_type")]
        public string productType { get; set; }

        [Required]
        [Column("product_description")]
        public string productDescription { get; set; }
    }
}
