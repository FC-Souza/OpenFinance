﻿using WebAPI.OpenFinance.Models;
using WebAPI.OpenFinance.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI.OpenFinance.Helpers;
using Microsoft.AspNetCore.SignalR;

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
        }
    }
}
