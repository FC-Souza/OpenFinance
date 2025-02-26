using Microsoft.EntityFrameworkCore;
using WebAPI.OpenFinance.Data;
using WebAPI.OpenFinance.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebAPI.OpenFinance.Helpers
{
    //Manage all the methods used to client portfolio details
    public static class ClientHelper
    {
        //Get all the connections for the clientID
        public static async Task<List<int>> GetClientConnectionsByClientID(OpenFinanceContext context, int clientID)
        {
            var clientConnections = await context.Connections
                //.Where(c => c.clientID == clientID)
                .Where(c => c.clientID == clientID && c.isActive)
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

        //Check if the client has any active connections
        public static async Task<bool> CheckClientConnections(OpenFinanceContext context, int clientID)
        {
            //Using AnyAsync instead of an if statement
            //return await context.Connections.AnyAsync(c => c.clientID == clientID);
            return await context.Connections.AnyAsync(c => c.clientID == clientID && c.isActive);
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

        //Get the total amount for Cash
        public static async Task<decimal> GetClientCashTotalAmount(OpenFinanceContext context, int clientID)
        {
            var clientConnections = await GetClientConnectionsByClientID(context, clientID);

            var cashTotal = await context.CashInfo
                .Where(c => clientConnections.Contains(c.connectionId))
                .SumAsync(c => c.amount);

            return cashTotal;
        }

        //Get the total amount for Stock 
        public static async Task<decimal> GetClienStockTotalAmount(OpenFinanceContext context, int clientID)
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

        //Get the total amount for Mutual Fund
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

        //Get the total amount of all the products for a ClientID
        public static async Task<decimal> GetClientTotalAmount(OpenFinanceContext context, int clientID)
        {
            var cashTotal = await GetClientCashTotalAmount(context, clientID);
            var stockTotal = await GetClienStockTotalAmount(context, clientID);
            var mutualFundTotal = await GetClientMutualFundTotalAmount(context, clientID);

            var totalAmount = cashTotal + stockTotal + mutualFundTotal;
            return totalAmount;
        }

        //Get the total amount for all the products for a ConnectionID
        public static async Task<decimal> GetConnectionTotalAmount(OpenFinanceContext context, int connectionID)
        {
            var cashTotal = await GetConnectionCashTotalAmount(context, connectionID);

            var stockTotal = await GetConnectionStockTotalAmount(context, connectionID);

            var mutualFundTotal = await GetConnectionMutualFundTotalAmount(context, connectionID);

            var totalAmount = cashTotal + stockTotal + mutualFundTotal;
            return totalAmount;
        }

        //Get the total amount for cash for a ConnectionID
        public static async Task<decimal> GetConnectionCashTotalAmount(OpenFinanceContext context, int connectionID)
        {
            var cashTotal = await context.CashInfo
                .Where(c => c.connectionId == connectionID)
                .SumAsync(c => c.amount);
            return cashTotal;
        }

        //Get the total amount for stock for a ConnectionID
        public static async Task<decimal> GetConnectionStockTotalAmount(OpenFinanceContext context, int connectionID)
        {
            var stockTotal = await context.StockInfo
                                .Where(si => si.connectionId == connectionID)
                                .Join(context.Stock,
                                    si => si.stockId,
                                    s => s.stockId,
                                    (si, s) => si.quantity * s.lastDayPrice)
                                .SumAsync();
            return stockTotal;
        }

        //Get the total amount for mutual fund for a ConnectionID
        public static async Task<decimal> GetConnectionMutualFundTotalAmount(OpenFinanceContext context, int connectionID)
        {
            var mutualFundTotal = await context.MutualFundInfo
                                    .Where(mf => mf.ConnectionID == connectionID)
                                    .Join(context.MutualFund,
                                        mf => mf.MFID,
                                        m => m.MFID,
                                        (mf, m) => mf.QuantityShares * m.MFNAV)
                                    .SumAsync();
            return mutualFundTotal;
        }

        //Get the connection details for a connectionID
        public static async Task<ConnectionDetails> GetConnectionDetails(OpenFinanceContext context, int connectionID)
        {
            //Select * from Connections where connectionID = connectionID
            var connection = await context.Connections
                .FirstOrDefaultAsync(c => c.connectionID == connectionID);

            var connectionDetail = new ConnectionDetails
            {
                BankName = await GetBankName(context, connection.bankID),
                BankID = connection.bankID,
                AccountNumber = connection.accountNumber,
                ConnectionID = connection.connectionID,
                ConnectionAmount = await GetConnectionTotalAmount(context, connectionID),
                ConnectionPercentage = 0,
                IsActive = connection.isActive
            };
            return connectionDetail;
        }

        //Get bank name for a bankID
        public static async Task<string> GetBankName(OpenFinanceContext context, int bankID)
        {
            var bankName = await context.Banks
                .Where(b => b.bankID == bankID)
                .Select(b => b.bankName)
                .FirstOrDefaultAsync();
            return bankName;
        }

        //Get the percentage for each connection
        public static void CalculatePercentageForEachConnection(List<ConnectionDetails> connectionDetails, decimal totalAmount)
        {
            foreach (var connection in connectionDetails)
            {
                if (connection.ConnectionAmount > 0)
                {
                    connection.ConnectionPercentage = Math.Round((connection.ConnectionAmount / totalAmount) * 100, 2);
                }
                else
                {
                    connection.ConnectionPercentage = 0;
                }
            }
        }

        //Get the connection details for all disable connection
        public static async Task<ConnectionDetails> GetDisablebConnectionDetails(OpenFinanceContext context, int connectionID)
        {
            //Select * from Connections where connectionID = connectionID
            var connection = await context.Connections
                .FirstOrDefaultAsync(c => c.connectionID == connectionID);


            var connectionDetail = new ConnectionDetails
            {
                BankName = await GetBankName(context, connection.bankID),
                BankID = connection.bankID,
                AccountNumber = connection.accountNumber,
                ConnectionID = connection.connectionID,
                ConnectionAmount = 0,
                ConnectionPercentage = 0,
                IsActive = connection.isActive
            };

            return connectionDetail;
        }

        //Get all the disable connections for a clientID
        public static async Task<List<int>> GetDisableClientConnectionsByClientID(OpenFinanceContext context, int clientID)
        {
            var disableClientConnections = await context.Connections
                .Where(c => c.clientID == clientID && !c.isActive)
                .Select(c => c.connectionID)
                .ToListAsync();
            return disableClientConnections;
        }

        //Check if the connection exists
        public static async Task<bool> CheckConnectionExists(OpenFinanceContext context, int clientID, int bankID, int accountNumber)
        {
            //Select * from connections where client_id = clientID and bank_id = bankID and account_number = accountNumber
            return await context.Connections
                .AnyAsync(c => c.clientID == clientID && c.bankID == bankID && c.accountNumber == accountNumber);
        }

        //Add a new connection
        public static async Task AddNewConnection(OpenFinanceContext context, int clientID, int bankID, int accountNumber)
        {
            var newConnection = new ConnectionsModel
            {
                clientID = clientID,
                bankID = bankID,
                accountNumber = accountNumber,
                isActive = true
            };

            context.Connections.Add(newConnection);

            await context.SaveChangesAsync();
        }

        //Check if the connection exists
        public static async Task<bool> CheckConnectionIDExists(OpenFinanceContext context,int clientID, int connectionID)
        {
            //Select * from connections where client_id = clientID and connection_id = connectionID
            return await context.Connections
                .AnyAsync(c => c.connectionID == connectionID && c.clientID == clientID);
        }

        //Enable or disable a connection
        public static async Task EnableDisableConnection(OpenFinanceContext context, int clientID, int connectionID, bool newStatus)
        {
            var connection = await context.Connections
                .FirstOrDefaultAsync(c => c.connectionID == connectionID && c.clientID == clientID);

            connection.isActive = newStatus;

            await context.SaveChangesAsync();
        }

        //Check if the connection is already disabled or enabled
        public static async Task<bool> CheckConnectionStatus(OpenFinanceContext context, int clientID, int connectionID, bool newStatus)
        {
            var connection = await context.Connections
                .FirstOrDefaultAsync(c => c.connectionID == connectionID && c.clientID == clientID);

            //Check if the connection is already disabled or enabled
            return connection.isActive == newStatus;
        }

        //[Overload] Calculate the percentage for each product 
        public static void CalculatePercentageForEachProduct(List<AssetsProductDetailsModel> productDetails)
        {

            decimal totalAmount = CalculateTotalAmount(productDetails);

            //Calculate the portfolio percentage for each product
            foreach (var product in productDetails)
            {
                if (product.ProductTotalAmount > 0)
                {
                    //product.PortfolioPercentage = (product.ProdTotal / totalAmount) * 100;
                    //Round to 2 decimal
                    product.PortfolioPercentage = Math.Round((product.ProductTotalAmount / totalAmount) * 100, 2);
                }
                else
                {
                    product.PortfolioPercentage = 0;
                }
            }
        }

        //[Overload] Calculate the total amount for all products
        public static decimal CalculateTotalAmount(List<AssetsProductDetailsModel> productDetails)
        {
            decimal totalAmount = 0;
            foreach (var product in productDetails)
            {
                totalAmount += product.ProductTotalAmount;
            }
            return totalAmount;
        }

        //[Overload] Get the number of products
        public static int GetNumProducts(List<AssetsProductDetailsModel> productDetails)
        {
            int numProducts = 0;
            foreach (var product in productDetails)
            {
                if (product.ProductTotalAmount > 0)
                {
                    numProducts++;
                }
            }
            return numProducts;
        }

        //Get the details for all stock items for a clientID
        public static async Task<List<AssetsProductItemModel>> GetStockItems(OpenFinanceContext context, int clientID)
        {
            var clientConnections = await GetClientConnectionsByClientID(context, clientID);

            var stockItems = await context.StockInfo
                .Where(si => clientConnections.Contains(si.connectionId))
                .Join(context.Stock,
                    si => si.stockId,
                    s => s.stockId,
                    (si, s) => new AssetsProductItemModel
                    {
                        ItemName = s.stockName,
                        ItemAmount = si.quantity * s.lastDayPrice
                        //ItemProfitLoss = await GetStockPercentageProfitLoss(context, si.averagePrice, si.quantity, s.lastDayPrice)
                    })
                .ToListAsync();

            return stockItems;
        }

        //Calculate the percentage profit loss for each stock item
        public static async Task CalculatePercentageProfitLossForEachStockItem(OpenFinanceContext context, List<AssetsProductItemModel> stockItems)
        {
            foreach (var stockItem in stockItems)
            {
                stockItem.ItemProfitLoss = await GetStockPercentageProfitLoss(context, stock, stockItem.ItemAmount);
            }
        }

        //Calculate the percentage profit loss for a stock item
        public static async Task<decimal> GetStockPercentageProfitLoss(OpenFinanceContext context, decimal averagePrice, int quantity, decimal lastDayPrice)
        {
            decimal profitLoss = (lastDayPrice - averagePrice) * quantity;

            decimal percentageProfitLoss = (profitLoss / (averagePrice * quantity)) * 100;

            return percentageProfitLoss;
        }

        //Get the details for all mutual funds items for a clientID
        public static async Task<List<AssetsProductItemModel>> GetMutualFundItems(OpenFinanceContext context, int clientID)
        {
            var clientConnections = await GetClientConnectionsByClientID(context, clientID);
            var mutualFundItems = await context.MutualFundInfo
                .Where(mfi => clientConnections.Contains(mfi.ConnectionID))
                .Join(context.MutualFund,
                    mfi => mfi.MFID,
                    mf => mf.MFID,
                    (mfi, mf) => new AssetsProductItemModel
                    {
                        ItemName = mf.MFName,
                        ItemAmount = mfi.QuantityShares * mf.MFNAV
                    })
                .ToListAsync();

            return mutualFundItems;
        }

        ////Calculate the percentage profit loss for a mutual fund item
        //public static async Task<decimal> GetMutualFundPercentageProfitLoss(OpenFinanceContext context, int quantityShares, decimal currentNAV, decimal averageNAV)
        //{
        //    decimal profitLoss = (currentNAV - averageNAV) * quantityShares;
        //    decimal percentageProfitLoss = (profitLoss / (averageNAV * quantityShares)) * 100;
        //    return percentageProfitLoss;
        //}



    }
}