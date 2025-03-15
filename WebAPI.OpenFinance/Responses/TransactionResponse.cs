namespace WebAPI.OpenFinance.Responses
{
    public class TransactionResponse
    {
        public int TransactionID { get; set; }
        public int ConnectionID { get; set; }
        public string TransactionType { get; set; }
        public string TransactionDirection { get; set; }
        public string AssetName { get; set; }
        public string TransactionDate { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}
