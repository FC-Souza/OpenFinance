using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.OpenFinance.Models
{
    [Table("client_credential")]
    public class ClientCredentialModel
    {
        /*
         * Table: client_credential
         * client_credential_id (PK, int, not null)
         * client_id (FK (client table), int, not null)
         * client_password (varchar(100), not null)
         * last_login (datetime)
         * remaining_login_attempts (int, DEFAULT 3)
         */

        [Key]
        [Column("client_credential_id")]
        public int clientCredentialID { get; init; }

        [Column("client_id")]
        public int clientID { get; set; }
        [ForeignKey("clientID")]
        public ClientsModel Client { get; set; }

        [Required]
        [Column("client_password")]
        public string clientPassword { get; set; }

        [Column("last_login")]
        public DateTime? lastLogin { get; set; }

        //Default value set at OnModelCreating
        [Column("remaining_login_attempts")]
        public int remainingLoginAttempts { get; set; }


    }
}
