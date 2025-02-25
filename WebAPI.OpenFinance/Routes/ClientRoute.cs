using WebAPI.OpenFinance.Models;
using WebAPI.OpenFinance.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI.OpenFinance.Helpers;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace WebAPI.OpenFinance.Routes
{
    public static class ClientRoute
    {
        public static void ClientRoutes(this WebApplication app)
        {

            var route = app.MapGroup("clients");

            //GET /clients/{clientID}/totalAmount
            //Will return a JSON with the total for each product, the total amount for all products, the clientID and the timestamp
            route.MapGet("/{clientID}/PortfolioTotalAmount", async (OpenFinanceContext context, int clientID) =>
            {
                //Will check the clientID in the Connections table and return the total amount of all products in the database for that connectionID

                //And the total amount for each product in the database for that clientID

                //Get all conectionId for the clientID
                //LINQ -> WHERE before SELECT
                var clientConnections = await ClientHelper.GetClientConnectionsByClientID(context, clientID);

                //Check if the client exists
                if (!await ClientHelper.CheckClientExists(context, clientID))
                {
                    return Results.BadRequest("Client not found");
                }

                //check if the client has any connections
                if (!await ClientHelper.CheckClientConnections(context, clientID))
                {
                    return Results.BadRequest("Client has no connections");
                }

                //Get the productTotal for each products
                //Create a list of ProductTotal
                var productTotals = new List<object>();
                decimal totalAmount = 0;

                //Sum CashInfo for each connection
                var cashTotal = await ClientHelper.GetClientCashTotalAmount(context, clientID);

                //Add the cashTotal to the totalAmount
                totalAmount += cashTotal;

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
                //var stockTotal = await context.StockInfo
                //    .Where(si => clientConnections.Contains(si.connectionId))
                //    .Join(context.Stock,
                //        si => si.stockId,
                //        s => s.stockId,
                //        (si, s) => si.quantity * s.lastDayPrice)
                //    .SumAsync();

                var stockTotal = await ClientHelper.GetClienStockTotalAmount(context, clientID);

                totalAmount += stockTotal;
                productTotals.Add(new { product = "Stock", total = stockTotal });

                //var mutualFundTotal = await context.MutualFundInfo
                //    .Where(mf => clientConnections.Contains(mf.ConnectionID))
                //    .Join(context.MutualFund,
                //        mf => mf.MFID,
                //        m => m.MFID,
                //        (mf, m) => mf.QuantityShares * m.MFNAV)
                //    .SumAsync();
                //totalAmount += mutualFundTotal;
                //productTotals.Add(new { product = "Mutual Fund", total = mutualFundTotal });

                var mutualFundTotal = await ClientHelper.GetClientMutualFundTotalAmount(context, clientID);
                totalAmount += mutualFundTotal;
                productTotals.Add(new { product = "Mutual Fund", total = mutualFundTotal });

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


            //GET /Clients/{clientID}/AssetsSummary
            /*
             * Receive the clientID
             * Check all the connections for the clientID
             * Check all the products for al the connections from the clientID
             * Calculate the total amount for each product
             * Calculate the % of representationn of each product
             * Return a JSON with the number of products, total amount portifolio, total amount for each product, the % of representation of each product, the clientID and the timestamp
             */
            route.MapGet("/{clientID}/AssetsSummary", async (OpenFinanceContext context, int clientID) =>
            {
                //Create s list for product details
                //var productDetails = new List<object>();
                var productDetail = new List<ProductDetails>();
                decimal totalAmount = 0;
                int numProducts = 0;

                //Check if the client exists
                if (!await ClientHelper.CheckClientExists(context, clientID))
                {
                    return Results.BadRequest("Client not found");
                }

                //Check if the client has any connections
                if (!await ClientHelper.CheckClientConnections(context, clientID))
                {
                    return Results.BadRequest("Client has no connections");
                }

                //Get all connections for the clientID
                var clientConnections = await ClientHelper.GetClientConnectionsByClientID(context, clientID);

                //STOCK
                var stockTotal = await ClientHelper.GetClienStockTotalAmount(context, clientID);

                //Add the stock details to the productDetails list
                productDetail.Add(new ProductDetails { ProductName = "Stock", ProdTotal = stockTotal });

                //MUTAL FUND
                var mutualFundTotal = await ClientHelper.GetClientMutualFundTotalAmount(context, clientID);
                productDetail.Add(new ProductDetails { ProductName = "Mutual Fund", ProdTotal = mutualFundTotal });

                //Calculate the % of representation of all products and add to the productDetails
                ClientHelper.CalculatePercentageForEachProduct(productDetail);

                //Get the number of products
                numProducts = ClientHelper.GetNumProducts(productDetail);

                //Get the total amount
                totalAmount = ClientHelper.CalculateTotalAmount(productDetail);

                //Check if the client has any products and return a message if not
                if (numProducts == 0)
                {
                    return Results.BadRequest("Client has no products");
                }

                //JSON response
                var response = new
                {
                    clientID = clientID,
                    numProducts = numProducts,
                    totalAmount = totalAmount,
                    productDetails = productDetail,
                    timestamp = DateTime.UtcNow
                };

                return Results.Ok(response);

            });

            //GET /Clients/{clientID}/GetAllConnections
            /*
             * Receive the clientID
             * Check if client exists
             * Check if the client has any connections
             * Calculate the total amount for all connections
             * Calculate the total amount for each connection
             * Calculate the percentage for each connection
             * Return a JSON with the numver of connections, total amount and connections details (bankName, bankID, accountNumber, amount, percentage)
             */
            route.MapGet("/{clientID}/GetAllConnections", async (OpenFinanceContext context, int clientID) =>
            {
                //Create a list for connection details
                var connectionDetail = new List<ConnectionDetails>();
                var numConnections = 0;

                //Check if the client exists
                if (!await ClientHelper.CheckClientExists(context, clientID))
                {
                    return Results.BadRequest("Client not found");
                }

                //Check if the client has any connections
                if (!await ClientHelper.CheckClientConnections(context, clientID))
                {
                    return Results.BadRequest("Client has no connections");
                }

                //Get all connections for the clientID
                var clientConnections = await ClientHelper.GetClientConnectionsByClientID(context, clientID);

                //Get the total amount for all connections
                var totalAmount = await ClientHelper.GetClientTotalAmount(context, clientID);

                //Get the number of connections
                numConnections = clientConnections.Count;

                //Loop through all connections to get the connection details
                foreach (var connection in clientConnections)
                {
                    //Get the connection details
                    var connectionDetails = await ClientHelper.GetConnectionDetails(context, connection);

                    //Add the connection details to the connectionDetail list
                    connectionDetail.Add(connectionDetails);
                }

                //Get the details for all disabled connections
                var disabledConnections = await ClientHelper.GetDisableClientConnectionsByClientID(context, clientID);

                //Loop through all disabled connections to get the connection details
                foreach (var connection in disabledConnections)
                {
                    //Get the connection details
                    var connectionDetails = await ClientHelper.GetDisablebConnectionDetails(context, connection);

                    //Add the connection details to the connectionDetail list
                    connectionDetail.Add(connectionDetails);
                }

                //Calculate the percentage for each connection
                ClientHelper.CalculatePercentageForEachConnection(connectionDetail, totalAmount);

                //JSON response
                var response = new
                {
                    clientID = clientID,
                    numConnections = numConnections,
                    totalAmount = totalAmount,
                    connections = connectionDetail,
                    timestamp = DateTime.UtcNow
                };

                return Results.Ok(response);
            });

            //POST /Clients/AddNewConnection
            /*
             * Receive the JSON with the clientID, bankID and accountNumber
             * Check if the client exists
             * Check if the connection already exists
             * Add the new connection
             * Return sucess message
             */
            route.MapPost("/AddNewConnection", async (OpenFinanceContext context, Connection connection) =>
            {
                //Check if the client exists
                if (!await ClientHelper.CheckClientExists(context, connection.ClientID))
                {
                    return Results.BadRequest("Client not found");
                }

                //Check if the connection already exists
                if (await ClientHelper.CheckConnectionExists(context, connection.ClientID, connection.BankID, connection.AccountNumber))
                {
                    return Results.BadRequest("Connection already exists. Check if the connection is disabled");
                }

                //Add the new connection
                await ClientHelper.AddNewConnection(context, connection.ClientID, connection.BankID, connection.AccountNumber);

                //Return success message
                return Results.Ok("Connection added successfully");
            });

            //PUT /Clients/EnableDisableConnection
            /*
             * Receive the JSON with the clientID, connectionID and status
             * Check if the client exists
             * Check if the connection exists
             * Update the connection status
             * Return sucess message
             */
            route.MapPut("/EnableDisableConnection", async (OpenFinanceContext context, UpdateConnection updateConnection) =>
            {
                Debug.WriteLine($"[START] Received request to update connection: ClientID={updateConnection.ClientID}, ConnectionID={updateConnection.ConnectionID}, NewStatus={updateConnection.Status}");


                //Check if the client exists
                if (!await ClientHelper.CheckClientExists(context, updateConnection.ClientID))
                {
                    Debug.WriteLine("[ERROR] Client not found");

                    return Results.BadRequest("Client not found");
                }

                Debug.WriteLine("[INFO] Client found");

                //Check if the connection exists
                if (!await ClientHelper.CheckConnectionIDExists(context, updateConnection.ClientID, updateConnection.ConnectionID))
                {
                    Debug.WriteLine("[ERROR] Connection not found for this client");

                    return Results.BadRequest("Connection not found for this client");
                }

                Debug.WriteLine("[INFO] Connection exists");

                //Check if the connection is already disabled/enabled
                if (await ClientHelper.CheckConnectionStatus(context, updateConnection.ClientID, updateConnection.ConnectionID, updateConnection.Status))
                {
                    Debug.WriteLine($"[DEBUG] Connection current status check: {updateConnection.Status}");

                    return Results.BadRequest("Connection is already disabled/enabled");
                }


                Debug.WriteLine("[INFO] Updating connection status...");

                //Update the connection status
                await ClientHelper.EnableDisableConnection(context, updateConnection.ClientID, updateConnection.ConnectionID, updateConnection.Status);

                Debug.WriteLine("[SUCCESS] Connection updated successfully");

                //Return success message
                return Results.Ok("Connection updated successfully");
            }); 
        }
    }

    //Class to receive the JSON from Add Connection
    public class Connection
    {
        public int ClientID { get; set; }
        public int BankID { get; set; }
        public int AccountNumber { get; set; }
    }

    //Class to receive the JSON from Update Connection
    public class UpdateConnection
    {
        public int ClientID { get; set; }
        public int ConnectionID { get; set; }
        public bool Status { get; set; }
    }
}