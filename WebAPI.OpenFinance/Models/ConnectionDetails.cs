namespace WebAPI.OpenFinance.Models
{
    public class ConnectionDetails
    {
        public string BankName { get; set; }
        public int BankID { get; set; }
        public int AccountNumber { get; set; }
        public decimal ConnectionAmount { get; set; }
        public decimal ConnectionPercentage { get; set; }
    }
}
