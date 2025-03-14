namespace WebAPI.OpenFinance.Responses
{
    public class ClientStatementResponse
    {
        public int ClientId { get; set; }
        public List<StatementByMonth> Statements { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
