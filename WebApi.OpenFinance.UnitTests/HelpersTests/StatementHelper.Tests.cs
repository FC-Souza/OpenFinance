using NUnit.Framework;
using WebAPI.OpenFinance.Helpers;
using WebAPI.OpenFinance.Models;
using WebAPI.OpenFinance.Responses;
using WebApi.OpenFinance.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApi.OpenFinance.UnitTests.HelpersTests
{
    [TestFixture]
    public class StatementHelperTests
    {
        [Test]
        public async Task GetTransactions_ValidClientIDAndPeriod_ReturnsTransactions()
        {
            // Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            var clientID = 1;
            var period = DateTime.UtcNow.AddMonths(-1);
            context.Clients.Add(new ClientsModel
            {
                clientID = clientID,
                clientName = "John Doe",
                clientEmail = "existing@example.com",
                clientAddress = "123 Main St"
            });
            context.Connections.Add(new ConnectionsModel
            {
                connectionID = 1,
                clientID = clientID,
                bankID = 1,
                accountNumber = 12345,
                isActive = true
            });
            context.Transaction.Add(new TransactionModel
            {
                TransactionID = 1,
                connectionId = 1,
                ProductID = 1,
                AssetName = "AAPL",
                TransactionDate = DateTime.UtcNow.AddDays(-10),
                TransactionAmount = 1000,
                TransactionType = new TransactionTypeModel { TransactionTypeName = "Buy" },
                TransactionDirection = new TransactionDirectionModel { TransactionDirectionName = "In" }
            });
            await context.SaveChangesAsync();

            // Act
            var result = await StatementHelper.GetTransactions(context, clientID, period);

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.First().TransactionID, Is.EqualTo(1));
        }

        [Test]
        public void GroupTransactionsByMonth_ValidTransactions_GroupsByMonth()
        {
            // Arrange
            var transactions = new List<TransactionResponse>
            {
                new TransactionResponse
                {
                    TransactionID = 1,
                    ConnectionID = 1,
                    TransactionType = "Buy",
                    TransactionDirection = "In",
                    AssetName = "AAPL",
                    TransactionDate = DateTime.UtcNow.AddMonths(-1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    TransactionAmount = 1000
                },
                new TransactionResponse
                {
                    TransactionID = 2,
                    ConnectionID = 1,
                    TransactionType = "Sell",
                    TransactionDirection = "Out",
                    AssetName = "AAPL",
                    TransactionDate = DateTime.UtcNow.AddMonths(-1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    TransactionAmount = 500
                }
            };

            // Act
            var result = StatementHelper.GroupTransactionsByMonth(transactions);

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.First().Month, Is.EqualTo(DateTime.UtcNow.AddMonths(-1).ToString("MMMM yyyy", new System.Globalization.CultureInfo("en-US"))));
            Assert.That(result.First().Transactions.Count, Is.EqualTo(2));
        }
    }
}
