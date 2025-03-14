namespace WebAPI.OpenFinance.Responses
{
    public class StatementByMonth
    {
        public string Month { get; set; }
        public List<TransactionDetail> Transactions { get; set; }
    }
}
