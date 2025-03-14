using WebAPI.OpenFinance.Models;
using WebAPI.OpenFinance.Data;
using WebAPI.OpenFinance.Helpers;
using System.Diagnostics;

namespace WebAPI.OpenFinance.Routes
{
    public static class StatementRoute
    {
        public static void StatementRoutes(this WebApplication app)
        {
            var route = app.MapGroup("statement");

            //GET /statement/{clientID}/ClientStatement
            //Receive the clientID
            //Return a JSON with the client's statement. Showing all the transactions for a clientID
            route.MapGet("/{clientID}/ClientStatement", async (OpenFinanceContext context, int clientID) =>
            {

                //Check if the client exists
                //Check if the client has a connection
                //Check if the client has a statement

                //Get all active connections for the clientID

                //Loop to get all the transactions by month

            });
        }
    }
}
