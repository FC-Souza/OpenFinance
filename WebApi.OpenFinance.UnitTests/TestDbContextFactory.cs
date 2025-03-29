using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using WebAPI.OpenFinance.Data;

namespace WebApi.OpenFinance.UnitTests.Helpers
{
    public static class TestDbContextFactory
    {
        public static OpenFinanceContext CreateInMemoryContext(string dbName = "TestDatabase")
        {
            var options = new DbContextOptionsBuilder<OpenFinanceContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new OpenFinanceContext(options);
            //Delete and clear the database every time a new context is created
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        private static void SeedDatabase(OpenFinanceContext context)
        {
            //Adding a client to the database for the tests
            context.Clients.Add(new WebAPI.OpenFinance.Models.ClientsModel
            {
                clientID = 1,
                clientName = "John Doe",
                clientEmail = "existing@example.com",
                clientAddress = "123 Main St"
            });

            context.SaveChanges();
        }
    }
}
