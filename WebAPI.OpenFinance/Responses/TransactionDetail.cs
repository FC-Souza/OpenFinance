namespace WebAPI.OpenFinance.Responses
{
    public class TransactionDetail
    {
        public int TransactionID { get; set; }
        public int StatementID { get; set; }
        public int ConnectionID { get; set; }
        public string TransactionType { get; set; }
        public string TransactionDirection { get; set; }
        public string ProductName { get; set; }
        public DateTime TransactionDate { get; set; }
        public Decimal TransactionAmount { get; set; }
    }
}
