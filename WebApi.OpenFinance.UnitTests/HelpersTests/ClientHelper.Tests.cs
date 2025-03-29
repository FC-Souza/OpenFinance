using NUnit.Framework;
using WebAPI.OpenFinance.Helpers;
using WebAPI.OpenFinance.Models;
using WebApi.OpenFinance.UnitTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApi.OpenFinance.UnitTests.HelpersTests
{
    [TestFixture]
    public class ClientHelperTests
    {
        [Test]
        public async Task CheckClientExists_ExistingClient_ReturnsTrue()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.Clients.Add(new ClientsModel
            {
                clientID = 1,
                clientName = "John Doe",
                clientEmail = "existing@example.com",
                clientAddress = "123 Main St"
            });
            await context.SaveChangesAsync();

            //Act
            var result = await ClientHelper.CheckClientExists(context, 1);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task CheckClientExists_NonExistingClient_ReturnsFalse()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();

            //Act
            var result = await ClientHelper.CheckClientExists(context, 999);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task CheckClientConnections_ExistingConnections_ReturnsTrue()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.Clients.Add(new ClientsModel
            {
                clientID = 1,
                clientName = "John Doe",
                clientEmail = "existing@example.com",
                clientAddress = "123 Main St"
            });
            context.Connections.Add(new ConnectionsModel
            {
                connectionID = 1,
                clientID = 1,
                bankID = 1,
                accountNumber = 12345,
                isActive = true
            });
            await context.SaveChangesAsync();

            //Act
            var result = await ClientHelper.CheckClientConnections(context, 1);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task CheckClientConnections_NoConnections_ReturnsFalse()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.Clients.Add(new ClientsModel
            {
                clientID = 1,
                clientName = "John Doe",
                clientEmail = "existing@example.com",
                clientAddress = "123 Main St"
            });
            await context.SaveChangesAsync();

            //Act
            var result = await ClientHelper.CheckClientConnections(context, 1);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetClientConnectionsByClientID_ValidClientID_ReturnsConnections()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.Clients.Add(new ClientsModel
            {
                clientID = 1,
                clientName = "John Doe",
                clientEmail = "existing@example.com",
                clientAddress = "123 Main St"
            });
            context.Connections.Add(new ConnectionsModel
            {
                connectionID = 1,
                clientID = 1,
                bankID = 1,
                accountNumber = 12345,
                isActive = true
            });
            await context.SaveChangesAsync();

            //Act
            var result = await ClientHelper.GetClientConnectionsByClientID(context, 1);

            //Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.First(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetClientConnectionsByClientID_InvalidClientID_ReturnsEmptyList()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();

            //Act
            var result = await ClientHelper.GetClientConnectionsByClientID(context, 999);

            //Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetClientCashTotalAmount_ValidClientID_ReturnsTotalAmount()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.Clients.Add(new ClientsModel
            {
                clientID = 1,
                clientName = "John Doe",
                clientEmail = "existing@example.com",
                clientAddress = "123 Main St"
            });
            context.Connections.Add(new ConnectionsModel
            {
                connectionID = 1,
                clientID = 1,
                bankID = 1,
                accountNumber = 12345,
                isActive = true
            });
            context.CashInfo.Add(new CashInfoModel
            {
                cashInfoId = 1,
                cashId = 1,
                connectionId = 1,
                amount = 1000
            });
            await context.SaveChangesAsync();

            //Act
            var result = await ClientHelper.GetClientCashTotalAmount(context, 1);

            //Assert
            Assert.That(result, Is.EqualTo(1000));
        }

        [Test]
        public async Task GetClientCashTotalAmount_InvalidClientID_ReturnsZero()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();

            //Act
            var result = await ClientHelper.GetClientCashTotalAmount(context, 999);

            //Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public async Task GetClienStockTotalAmount_ValidClientID_ReturnsTotalAmount()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.Clients.Add(new ClientsModel
            {
                clientID = 1,
                clientName = "John Doe",
                clientEmail = "existing@example.com",
                clientAddress = "123 Main St"
            });
            context.Connections.Add(new ConnectionsModel
            {
                connectionID = 1,
                clientID = 1,
                bankID = 1,
                accountNumber = 12345,
                isActive = true
            });
            context.Stock.Add(new StockModel
            {
                stockId = 1,
                productId = 2,
                ticker = "AAPL",
                stockName = "Apple Inc.",
                ISIN = "US0378331005",
                lastDayPrice = 150,
                currency = "USD",
                exchange = "NASDAQ"
            });
            context.StockInfo.Add(new StockInfoModel
            {
                stockInfoId = 1,
                stockId = 1,
                connectionId = 1,
                quantity = 10,
                averagePrice = 100,
                lastUpdated = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            //Act
            var result = await ClientHelper.GetClienStockTotalAmount(context, 1);

            //Assert
            Assert.That(result, Is.EqualTo(1500));
        }

        [Test]
        public async Task GetClienStockTotalAmount_InvalidClientID_ReturnsZero()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();

            //Act
            var result = await ClientHelper.GetClienStockTotalAmount(context, 999);

            //Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public async Task GetClientMutualFundTotalAmount_ValidClientID_ReturnsTotalAmount()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.Clients.Add(new ClientsModel
            {
                clientID = 1,
                clientName = "John Doe",
                clientEmail = "existing@example.com",
                clientAddress = "123 Main St"
            });
            context.Connections.Add(new ConnectionsModel
            {
                connectionID = 1,
                clientID = 1,
                bankID = 1,
                accountNumber = 12345,
                isActive = true
            });
            context.MutualFund.Add(new MutualFundModel
            {
                MFID = 1,
                productId = 3,
                MFName = "Vanguard 500 Index Fund",
                MFSymbol = "VFIAX",
                MFType = "Index Fund",
                MFCurrency = "USD",
                MFNAV = 350,
                MFInceptionDate = new DateOnly(2000, 1, 1),
                MFManagementFee = 0.04m
            });
            context.MutualFundInfo.Add(new MutualFundInfoModel
            {
                MFIID = 1,
                MFID = 1,
                ConnectionID = 1,
                QuantityShares = 10,
                AverageNAV = 300,
                LastUpdated = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            //Act
            var result = await ClientHelper.GetClientMutualFundTotalAmount(context, 1);

            //Assert
            Assert.That(result, Is.EqualTo(3500));
        }

        [Test]
        public async Task GetClientMutualFundTotalAmount_InvalidClientID_ReturnsZero()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();

            //Act
            var result = await ClientHelper.GetClientMutualFundTotalAmount(context, 999);

            //Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public async Task GetClientTotalAmount_ValidClientID_ReturnsTotalAmount()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.Clients.Add(new ClientsModel
            {
                clientID = 1,
                clientName = "John Doe",
                clientEmail = "existing@example.com",
                clientAddress = "123 Main St"
            });
            context.Connections.Add(new ConnectionsModel
            {
                connectionID = 1,
                clientID = 1,
                bankID = 1,
                accountNumber = 12345,
                isActive = true
            });
            context.CashInfo.Add(new CashInfoModel
            {
                cashInfoId = 1,
                cashId = 1,
                connectionId = 1,
                amount = 1000
            });
            context.Stock.Add(new StockModel
            {
                stockId = 1,
                productId = 2,
                ticker = "AAPL",
                stockName = "Apple Inc.",
                ISIN = "US0378331005",
                lastDayPrice = 150,
                currency = "USD",
                exchange = "NASDAQ"
            });
            context.StockInfo.Add(new StockInfoModel
            {
                stockInfoId = 1,
                stockId = 1,
                connectionId = 1,
                quantity = 10,
                averagePrice = 100,
                lastUpdated = DateTime.UtcNow
            });
            context.MutualFund.Add(new MutualFundModel
            {
                MFID = 1,
                productId = 3,
                MFName = "Vanguard 500 Index Fund",
                MFSymbol = "VFIAX",
                MFType = "Index Fund",
                MFCurrency = "USD",
                MFNAV = 350,
                MFInceptionDate = new DateOnly(2000, 1, 1),
                MFManagementFee = 0.04m
            });
            context.MutualFundInfo.Add(new MutualFundInfoModel
            {
                MFIID = 1,
                MFID = 1,
                ConnectionID = 1,
                QuantityShares = 10,
                AverageNAV = 300,
                LastUpdated = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            //Act
            var result = await ClientHelper.GetClientTotalAmount(context, 1);

            //Assert
            Assert.That(result, Is.EqualTo(6000));
        }

        [Test]
        public async Task GetClientTotalAmount_InvalidClientID_ReturnsZero()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();

            //Act
            var result = await ClientHelper.GetClientTotalAmount(context, 999);

            //Assert
            Assert.That(result, Is.EqualTo(0));
        }
    }
}
