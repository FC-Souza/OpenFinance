using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebAPI.OpenFinance.Data;
using WebAPI.OpenFinance.Models;
using WebAPI.OpenFinance.Responses;

namespace WebAPI.OpenFinance.Helpers
{
    //Manage all the methods used to help the StatementRoute
    public static class StatementHelper
    {

        //Get all the transactions for the clientID and period
        public static async Task<List<TransactionResponse>> GetTransactions(OpenFinanceContext context, int clientID, DateTime period)
        {
            return await context.Transaction
                .Where(t => t.Connection.clientID == clientID && t.TransactionDate >= period)
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => new TransactionResponse
                    {
                        TransactionID = t.TransactionID,
                        ConnectionID = t.connectionId,
                        TransactionType = t.TransactionType.TransactionTypeName,
                        TransactionDirection = t.TransactionDirection.TransactionDirectionName,
                        AssetName = t.AssetName,
                        TransactionDate = t.TransactionDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        TransactionAmount = t.TransactionAmount
                    })
                .ToListAsync();
        }

        //Group the transactions by month
        public static List<StatementResponse> GroupTransactionsByMonth(List<TransactionResponse> transactions)
        {
            return transactions
                .GroupBy(t => DateTime.Parse(t.TransactionDate)
                //CultureInfo("en-US") to ensure the month is in english
                .ToString("MMMM yyyy", new CultureInfo("en-US")))
                .Select(g => new StatementResponse
                    {
                        Month = g.Key,
                        Transactions = g.ToList()
                    })
                .ToList();
        }


    }
}
