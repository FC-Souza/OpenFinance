using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("statement")]
    public class StatementModel
    {
        [Key]
        [Column("statement_id")]
        public int StatementID { get; set; }

        [Required]
        [Column("connection_id")]
        public int connectionID { get; set; }
        [ForeignKey("connectionId")]
        public ConnectionsModel Connection { get; set; }

        [Required]
        [Column("statement_month")]
        public int StatementMonth { get; set; }

        [Required]
        [Column("statement_year")]
        public int StatementYear { get; set; }

        [Required]
        [Column("last_update")]
        public DateTime LastUpdate { get; set; }
    }
}
