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

                //Sum StockInfo for all connections
                /*
                 * Get all stockInfo for the clientConnections
                 * Get the last_day_price of all stocks at stock table
                 * Multiply the quantity * LastDayPrice
                 * Sum the mutiplication result
                 * Add the stockTotal to the totalAmount
                 * Add the stockTotal to the productTotals as Stock
                 */

                /*
                    select 
                    sum(si.quantity * s.last_day_price) as total
                    from stock_info si
                    join stock s
                    on si.stock_id = s.stock_id
                    where connection_id in 
                    (
                        select
                        connection_id
                        from connections
                        where client_id = 1
                    )
                */

                var stockTotal = await context.StockInfo
                    .Where(si => clientConnections.Contains(si.connectionId))
                    .Join(context.Stock,
                        si => si.stockId,
                        s => s.stockId,
                        (si, s) => si.quantity * s.lastDayPrice)
                    .SumAsync();

                totalAmount += stockTotal;
                productTotals.Add(new { product = "Stock", total = stockTotal });


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
