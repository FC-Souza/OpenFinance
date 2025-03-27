using WebAPI.OpenFinance.Data;
using WebAPI.OpenFinance.Helpers;
using WebAPI.OpenFinance.Responses;

namespace WebAPI.OpenFinance.Routes
{
    public static class ReportRoute
    {
        public static void ReportRoutes(this WebApplication app)
        {
            var route = app.MapGroup("report");


            //GET /report/{reportPeriod}/GenerateProfitReport
            //Trigger to generate the profit report for the period
            route.MapGet("/{reportPeriod}/GenerateProfitReport", async (OpenFinanceContext context, DateTime reportPeriod) =>
            {
                await ReportHelper.GenerateProfitReport(context, reportPeriod);
                return Results.Ok("Report generated");
            });

            //GET /report/{clientID}/ProfitReport
            //Receive the clientID
            //Will return the profit report for the clientID for the last 12 months
            //Return a JSON with the client's profit report. Showing all the profit/loss for a clientID
            route.MapGet("/{clientID}/ProfitReport", async (OpenFinanceContext context, int clientID) =>
            {

                //Check if the client exists
                if (!await ClientHelper.CheckClientExists(context, clientID))
                {
                    return Results.BadRequest("Client not found");
                }

                //Check if the has active connections
                if (!await ClientHelper.CheckClientConnections(context, clientID))
                {
                    return Results.BadRequest("Client has no active connections");
                }

                //Get the profit report for the clientID
                var reportByMonth = await ReportHelper.GetProfitReport(context, clientID);


                var response = new ProfitReportResponse
                {
                    ClientID = clientID,
                    ProfitReportByMonth = reportByMonth,
                    Timestamp = DateTime.UtcNow
                };



                return Results.Ok(response);
            });

            //GET /report/BenchmarkIndex
            //Will return the benchmark index for the last 12 months
            //Return a JSON with the benchmark index for the last 12 months
            route.MapGet("/BenchmarkIndex", async (OpenFinanceContext context) =>
            {
                var benchmarkIndexes = await ReportHelper.GetBenchmarkIndex(context);

                if (benchmarkIndexes == null)
                {
                    return Results.BadRequest("No benchmark index found for the period");
                }

                var response = new BenchmarkIndexResponse
                {
                    BenchmarkIndexes = benchmarkIndexes,
                    Timestamp = DateTime.UtcNow
                };

                return Results.Ok(response);
            });
        }
    }
}
