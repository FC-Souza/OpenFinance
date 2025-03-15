using WebAPI.OpenFinance.Models;
using WebAPI.OpenFinance.Data;
using WebAPI.OpenFinance.Helpers;
using System.Diagnostics;
using WebAPI.OpenFinance.Responses;

namespace WebAPI.OpenFinance.Routes
{
    public static class StatementRoute
    {
        public static void StatementRoutes(this WebApplication app)
        {
            var route = app.MapGroup("statement");

            //GET /statement/{clientID}/ClientStatement
            //Receive the clientID
            //Will return the transactions for the clientID only for the last 12 months
            //Will return the transactions for the clientID separeted by month
            //Return a JSON with the client's statement. Showing all the transactions for a clientID
            route.MapGet("/{clientID}/ClientStatement", async (OpenFinanceContext context, int clientID) =>
            {

                //Check if the client exists
                if (!await ClientHelper.CheckClientExists(context, clientID))
                {
                    return Results.BadRequest("Client not found");
                }

                //Check if the has active connections
                if (!await ClientHelper.CheckClientConnections(context, clientID))
                {
                    return Results.BadRequest("Client has no connections");
                }

                //Get the period
                var period = DateTime.UtcNow.AddMonths(-12);

                //Get all the transactions for the period
                var transactions = await StatementHelper.GetTransactions(context, clientID, period);

                if (!transactions.Any())
                {
                    return Results.BadRequest("Client has no transactions for the period");
                }

                //Group the transactions by month
                var groupedTransactions = StatementHelper.GroupTransactionsByMonth(transactions);

                //JSON
                var response = new
                {
                    ClientID = clientID,
                    Statement = groupedTransactions,
                    Timestamp = DateTime.UtcNow
                };

                return Results.Ok(response);
            });
        }
    }
}
