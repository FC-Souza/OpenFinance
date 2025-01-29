using WebAPI.OpenFinance.Models;
using WebAPI.OpenFinance.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.OpenFinance.Routes
{
    public static class ClientRoute
    {
        public static void ClientRoutes(this WebApplication app)
        {

            var route = app.MapGroup("clients");

            //GET /clients/{clientID}/totalAmount
            //Will return a JSON with the total for each product, the total amount for all products, the clientID and the timestamp
            route.MapGet("/{clientID}/PortifolioTotalAmount", async (OpenFinanceContext context, int clientID) =>
            {
                //Will check the clientID in the Connections table and return the total amount of all products in the database for that connectionID
                
                //And the total amount for each product in the database for that clientID
                
                //Get all conectionId for the clientID
                //LINQ -> WHERE before SELECT
                var clientConnections = await context.Connections
                    .Where(c => c.clientID == clientID)
                    .Select(c => c.connectionID)
                    .ToListAsync();

                //Check if the client exists

                //check if the client has any connections

                //Get the productTotal for each products
                //Create a list of ProductTotal
                var productTotals = new List<object>();
                decimal totalAmount = 0;

                //Sum CashInfo for each connection
                var cashTotal = await context.CashInfo
                    .Where(c => clientConnections.Contains(c.connectionId))
                    .SumAsync(c => c.amount);
                totalAmount += cashTotal;
                productTotals.Add(new { product = "Cash", total = cashTotal });

                //Sum StockInfo for each connection
                //Sum FundsInfo for each connection


                //Return a JSON with the total for each product, the total amount for all products, the clientID and the timestamp
                //Creating the JSON response
                var response = new
                {
                    clientID = clientID,
                    totalAmount = totalAmount,
                    productTotals = productTotals,
                    //Use UTC as timestamp ALWAYS. Front and Back
                    timestamp = DateTime.UtcNow
                };
                //Returning the response
                return Results.Ok(response);
            });

        }
    }
}
