using WebAPI.OpenFinance.Models;
using WebAPI.OpenFinance.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.OpenFinance.Routes
{
    public static class BanksListRoute
    {
        public static void BanksListRoutes(this WebApplication app)
        {

            var route = app.MapGroup("bankslist");

            //GET /bankslist
            //Return all banks in a JSON format
            route.MapGet("/", async (OpenFinanceContext context) =>
            {
                var banks = await context.Banks.ToListAsync();
                return Results.Ok(banks);

            });

            //POST /bankslist
            //Add a new bank to the database
            //Will receive a JSON with the bank information. bank_ID and bank_name
            route.MapPost("/addBanks", async (OpenFinanceContext context, List<BanksModel> banks) =>
            {
                context.Banks.AddRange(banks);
                await context.SaveChangesAsync();
                return Results.Ok("Banks inserted!");
            });

        }
    }
}
