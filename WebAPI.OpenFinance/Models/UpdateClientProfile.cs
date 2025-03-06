using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.OpenFinance.Models
{
    public class UpdateClientProfile
    {

        [Required]
        public int clientID { get; init; }

        public string? clientName { get; set; }

        public string? clientEmail { get; set; }

        public string? clientAddress { get; set; }
    }
}
