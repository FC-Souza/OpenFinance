using WebAPI.OpenFinance.Data;
using WebAPI.OpenFinance.Helpers;

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

            //GET /report/{reportPeriod}/ProfitReport
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



                var report = await ReportHelper.GetProfitReport(context, clientID);


                return Results.Ok(report);
            });
        }
    }
}
