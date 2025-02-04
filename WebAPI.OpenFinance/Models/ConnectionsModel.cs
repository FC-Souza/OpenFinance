using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("connections")]
    public class ConnectionsModel
    {
        /*
         * Table: connections
         * connection_id (PK, int, not null)
         * client_id (FK (clients table), int, not null)
         * bank_id (FK (banks table), int, not null)
         * account_number (int, not null)
         */

        [Key]
        [Column("connection_id")]
        public int connectionID { get; init; }

        [Column("client_id")]
        public int clientID { get; set; }
        [ForeignKey("clientID")]
        public ClientsModel Client { get; set; }

        [ForeignKey("banks")]
        [Column("bank_id")]
        public int bankID { get; set; }

        [Required]
        [Column("account_number")]
        public int accountNumber { get; set; }
    }
}