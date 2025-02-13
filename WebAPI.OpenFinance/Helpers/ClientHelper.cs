using Microsoft.EntityFrameworkCore;
using WebAPI.OpenFinance.Data;
using WebAPI.OpenFinance.Models;

namespace WebAPI.OpenFinance.Helpers
{
    //Manage all the methods used to client portfolio details
    public static class ClientHelper
    {
        //Get all the connections for the clientID
        public static async Task<List<int>> GetClientConnectionsByClientID(OpenFinanceContext context, int clientID)
        {
            var clientConnections = await context.Connections
                .Where(c => c.clientID == clientID)
                .Select(c => c.connectionID)
                .ToListAsync();
            return clientConnections;
        }

        //Check if the client exists
        public static async Task<bool> CheckClientExists(OpenFinanceContext context, int clientID)
        {
            //Using AnyAsync instead of an if statement
            return await context.Clients.AnyAsync(c => c.clientID == clientID);
        }

        //Check if the client has any connections
        public static async Task<bool> CheckClientConnections(OpenFinanceContext context, int clientID)
        {
            //Using AnyAsync instead of an if statement
            return await context.Connections.AnyAsync(c => c.clientID == clientID);
        }

        //Get stock total amount for the clientID
        public static async Task<decimal> GetClientStockTotalAmount(OpenFinanceContext context, int clientID)
        {
            var clientConnections = await GetClientConnectionsByClientID(context, clientID);

            var stockTotal = await context.StockInfo
                    .Where(si => clientConnections.Contains(si.connectionId))
                    .Join(context.Stock,
                        si => si.stockId,
                        s => s.stockId,
                        (si, s) => si.quantity * s.lastDayPrice)
                    .SumAsync();

            return stockTotal;
        }

        //Get mutual fund total amount for the clientID
        public static async Task<decimal> GetClientMutualFundTotalAmount(OpenFinanceContext context, int clientID)
        {
            var clientConnections = await GetClientConnectionsByClientID(context, clientID);
            var mutualFundTotal = await context.MutualFundInfo
                    .Where(mf => clientConnections.Contains(mf.ConnectionID))
                    .Join(context.MutualFund,
                        mf => mf.MFID,
                        m => m.MFID,
                        (mf, m) => mf.QuantityShares * m.MFNAV)
                    .SumAsync();
            return mutualFundTotal;
        }

        //Calculate the percentage for each product
        public static void CalculatePercentageForEachProduct(List<ProductDetails> productDetails)
        {
            //Get the totalAmount fo all the products
            //decimal totalAmount = 0;
            //foreach (var product in productDetails)
            //{
            //    totalAmount += product.ProdTotal;
            //}
            decimal totalAmount = CalculateTotalAmount(productDetails);

            //Calculate the portfolio percentage for each product
            foreach (var product in productDetails)
            {
                if (product.ProdTotal > 0)
                {
                    //product.PortfolioPercentage = (product.ProdTotal / totalAmount) * 100;
                    //Round to 2 decimal
                    product.PortfolioPercentage = Math.Round((product.ProdTotal / totalAmount) * 100, 2);
                }
                else
                {
                    product.PortfolioPercentage = 0;
                }
            }
        }

        //Calculate the total amount for all products
        public static decimal CalculateTotalAmount(List<ProductDetails> productDetails)
        {
            decimal totalAmount = 0;
            foreach (var product in productDetails)
            {
                totalAmount += product.ProdTotal;
            }
            return totalAmount;
        }

        //Get the number of products
        public static int GetNumProducts(List<ProductDetails> productDetails)
        {
            int numProducts = 0;
            foreach (var product in productDetails)
            {
                if (product.ProdTotal > 0)
                {
                    numProducts++;
                }
            }
            return numProducts;
        }

    }
}
