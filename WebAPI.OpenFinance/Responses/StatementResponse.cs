namespace WebAPI.OpenFinance.Responses
{
    public class StatementResponse
    {
        public string Month { get; set; }
        public List<TransactionResponse> Transactions { get; set; }
    }
}
