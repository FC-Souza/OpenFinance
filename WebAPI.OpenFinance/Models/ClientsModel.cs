using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("clients")]
    public class ClientsModel
    {
        /*
         * Table: clients
         * client_id (PK, int, not null)
         * client_name (varchar(100), not null)
         * client_email (varchar(100), not null)
         * client_address (varchar(100), not null)
         */

        [Key]
        [Column("client_id")]
        public int clientID { get; init; }

        [Required]
        [Column("client_name")]
        public string clientName { get; set; }

        [Required]
        [Column("client_email")]
        public string clientEmail { get; set; }

        [Required]
        [Column("client_address")]
        public string clientAddress { get; set; }
    }
}
