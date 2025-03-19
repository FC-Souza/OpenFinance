using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebAPI.OpenFinance.Data;
using WebAPI.OpenFinance.Models;

namespace WebAPI.OpenFinance.Helpers
{
    //Manage all the methods that will be used to generate reports
    public static class ReportHelper
    {
        //Generate the profit report
        //Calculate the profit/loss for each connectionID grouped by productID
        //Calculate the total amount invested, total amount, total profit/loss and total profit/loss percentage
        //Calculate everything just for the currently month
        //Save the report in the profit_report table
        public static async Task GenerateProfitReport(OpenFinanceContext context, DateTime reportPeriod)
        {
            //Get all the clients with active connections
            var clients = await ClientHelper.GetActiveClientIDs(context);

            //Calculate the profit/loss for each connectionID grouped by productID
            foreach (var clientID in clients)
            {
                await CalculateStockProfitLoss(context, clientID, reportPeriod);

                await CalculateMutualFundProfitLoss(context, clientID, reportPeriod);
            }

        }

        //Calculae the total amount invested, total amount, total profit/loss and total profit/loss percentage for Mutual Funds
        public static async Task CalculateStockProfitLoss(OpenFinanceContext context, int clientID, DateTime reportPeriod)
        {
            //Get the connections for the clientID
            var clientConnections = await ClientHelper.GetClientConnectionsByClientID(context, clientID);


            //SELECT
            //    s.product_id AS product_id
            //    , SUM(si.quantity * si.average_price) AS total_amount_invested
            //    , SUM(si.quantity * s.last_day_price) AS total_amount
            //    , SUM(si.quantity * s.last_day_price) -SUM(si.quantity * si.average_price) AS total_profit_loss
            //    , (SUM(si.quantity * s.last_day_price) - SUM(si.quantity * si.average_price)) / SUM(si.quantity * si.average_price) * 100 AS total_profit_loss_percentage
            //FROM stock s
            //JOIN stock_info si ON s.stock_id = si.stock_id
            //WHERE si.connection_id IN(1,2)
            //GROUP BY s.product_id;

            //Get the total amount invested, total amount, total profit/loss and total profit/loss percentage for each connectionID
            var results = await context.StockInfo
                .Where(si => clientConnections.Contains(si.connectionId))
                .Join(
                    context.Stock,
                    si => si.stockId,
                    s => s.stockId,
                    (si, s) => new { si, s }
                )
                .Where(x => x.s.productId == 2)
                .GroupBy(x => new { x.si.connectionId, x.s.productId })
                .Select(g => new
                {
                    ConnectionId = g.Key.connectionId,
                    ProductId = g.Key.productId,
                    TotalAmountInvested = g.Sum(x => x.si.quantity * x.si.averagePrice),
                    TotalAmount = g.Sum(x => x.si.quantity * x.s.lastDayPrice),
                    TotalProfitLoss = g.Sum(x => x.si.quantity * x.s.lastDayPrice) - g.Sum(x => x.si.quantity * x.si.averagePrice),
                    TotalProfitLossPercentage = g.Sum(x => x.si.quantity * x.si.averagePrice) == 0
                        ? 0
                        : Math.Round(((g.Sum(x => x.si.quantity * x.s.lastDayPrice) - g.Sum(x => x.si.quantity * x.si.averagePrice))
                                     / g.Sum(x => x.si.quantity * x.si.averagePrice)) * 100, 2)
                })
                .ToListAsync();


            //Save each connectionID in the profit_report table
            foreach (var r in results)
            {

                var profitReport = new ProfitReportModel
                {
                    connectionId = r.ConnectionId,
                    productId = r.ProductId,
                    TotalAmountInvested = r.TotalAmountInvested,
                    TotalAmount = r.TotalAmount,
                    TotalProfit = r.TotalProfitLoss,
                    TotalProfitPercentage = r.TotalProfitLossPercentage,
                    ReportPeriod = reportPeriod,
                    ReportDate = DateTime.UtcNow.Date
                };

                context.ProfitReport.Add(profitReport);
            }

            await context.SaveChangesAsync();
        }



        //Calculae the total amount invested, total amount, total profit/loss and total profit/loss percentage for Stock
        public static async Task CalculateMutualFundProfitLoss(OpenFinanceContext context, int clientID, DateTime reportPeriod)
        {
            //Get the connections for the clientID
            var clientConnections = await ClientHelper.GetClientConnectionsByClientID(context, clientID);


            //SELECT
            //    mf.product_id AS product_id
            //    , SUM(mfi.quantity_shares * mfi.average_nav) AS total_amount_invested
            //    , SUM(mfi.quantity_shares * mf.mf_last_nav) AS total_amount
            //    , SUM(mfi.quantity_shares * mf.mf_last_nav) -SUM(mfi.quantity_shares * mfi.average_nav) AS total_profit_loss
            //    , (SUM(mfi.quantity_shares * mf.mf_last_nav) - SUM(mfi.quantity_shares * mfi.average_nav)) / SUM(mfi.quantity_shares * mfi.average_nav) * 100 AS total_profit_loss_percentage
            //FROM mutual_fund mf
            //JOIN mutual_fund_info mfi ON mf.mf_id = mfi.mf_id
            //WHERE mfi.connection_id IN(1,2)
            //GROUP BY mf.product_id;

            //Get the total amount invested, total amount, total profit/loss and total profit/loss percentage for each connectionID

            var results = await context.MutualFundInfo
                .Where(mfi => clientConnections.Contains(mfi.ConnectionID))
                .Join(
                    context.MutualFund,
                    mfi => mfi.MFIID,
                    mf => mf.MFID,
                    (mfi, mf) => new { mfi, mf }
                )
                .Where(x => x.mf.productId == 3)
                .GroupBy(x => new { x.mfi.ConnectionID, x.mf.productId })
                .Select(g => new
                {
                    ConnectionId = g.Key.ConnectionID,
                    ProductId = g.Key.productId,
                    TotalAmountInvested = g.Sum(x => x.mfi.QuantityShares * x.mfi.AverageNAV),
                    TotalAmount = g.Sum(x => x.mfi.QuantityShares * x.mf.MFNAV),
                    TotalProfitLoss = g.Sum(x => x.mfi.QuantityShares * x.mf.MFNAV) - g.Sum(x => x.mfi.QuantityShares * x.mfi.AverageNAV),
                    TotalProfitLossPercentage = g.Sum(x => x.mfi.QuantityShares * x.mfi.AverageNAV) == 0
                        ? 0
                        : Math.Round(((g.Sum(x => x.mfi.QuantityShares * x.mf.MFNAV) - g.Sum(x => x.mfi.QuantityShares * x.mfi.AverageNAV))
                                     / g.Sum(x => x.mfi.QuantityShares * x.mfi.AverageNAV)) * 100, 2)
                })
                .ToListAsync();



            //Save each connectionID in the profit_report table
            foreach (var r in results)
            {

                var profitReport = new ProfitReportModel
                {
                    connectionId = r.ConnectionId,
                    productId = r.ProductId,
                    TotalAmountInvested = r.TotalAmountInvested,
                    TotalAmount = r.TotalAmount,
                    TotalProfit = r.TotalProfitLoss,
                    TotalProfitPercentage = r.TotalProfitLossPercentage,
                    ReportPeriod = reportPeriod,
                    ReportDate = DateTime.UtcNow.Date
                };

                context.ProfitReport.Add(profitReport);
            }

            await context.SaveChangesAsync();
        }

    }



}
