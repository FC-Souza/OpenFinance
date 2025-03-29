using Moq;
using Microsoft.EntityFrameworkCore;
using WebAPI.OpenFinance.Data;
using WebAPI.OpenFinance.Helpers;
using WebAPI.OpenFinance.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.OpenFinance.UnitTests;

namespace WebApi.OpenFinance.UnitTests.HelpersTests
{
    [TestFixture]
    public class ReportHelperTests
    {
        [Test]
        public async Task GenerateProfitReport_ValidData_GeneratesReport()
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

            var reportPeriod = DateTime.UtcNow;

            //Act
            await ReportHelper.GenerateProfitReport(context, reportPeriod);

            //Assert
            var report = await context.ProfitReport.FirstOrDefaultAsync();
            Assert.That(report, Is.Not.Null);
            Assert.That(report.TotalAmountInvested, Is.GreaterThan(0));
            Assert.That(report.TotalAmount, Is.GreaterThan(0));
            Assert.That(report.TotalProfit, Is.GreaterThan(0));
        }

        [Test]
        public async Task CalculateStockProfitLoss_ValidClientID_SavesProfitReport()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            var reportPeriod = new DateTime(2024, 03, 01);

            var clientID = 1;
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
            await ReportHelper.CalculateStockProfitLoss(context, clientID, reportPeriod);

            //Assert
            var report = await context.ProfitReport.FirstOrDefaultAsync();
            Assert.That(report, Is.Not.Null);
        }

        [Test]
        public async Task CalculateMutualFundProfitLoss_ValidClientID_SavesProfitReport()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            var reportPeriod = new DateTime(2024, 03, 01);

            var clientID = 1;
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
            await ReportHelper.CalculateMutualFundProfitLoss(context, clientID, reportPeriod);

            //Assert
            var report = await context.ProfitReport.FirstOrDefaultAsync();
            Assert.That(report, Is.Not.Null);
        }

        [Test]
        public async Task GetProfitReport_ValidClientID_ReturnsReportList()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            var clientID = 1;
            var reportPeriod = DateTime.UtcNow.AddMonths(-6);
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
            context.ProfitReport.Add(new ProfitReportModel
            {
                connectionId = 1,
                productId = 2,
                TotalAmountInvested = 1000,
                TotalAmount = 1500,
                TotalProfit = 500,
                TotalProfitPercentage = 50,
                ReportPeriod = reportPeriod,
                ReportDate = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            //Act
            var result = await ReportHelper.GetProfitReport(context, clientID);

            //Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.First().TotalAmountInvested, Is.EqualTo(1000));
        }

        [Test]
        public async Task GetBenchmarkIndex_ReturnsBenchmarkList()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            var benchmarkList = new List<BenchmarkIndexModel>
            {
                new BenchmarkIndexModel { BenchmarkPeriod = DateTime.UtcNow.AddMonths(-3), CPI = 2.1m, SPTSX = 1.2m, CBPR = 4.5m },
                new BenchmarkIndexModel { BenchmarkPeriod = DateTime.UtcNow.AddMonths(-2), CPI = 1.9m, SPTSX = 1.7m, CBPR = 4.3m }
            };
            context.BenchmarkIndex.AddRange(benchmarkList);
            await context.SaveChangesAsync();

            //Act
            var result = await ReportHelper.GetBenchmarkIndex(context);

            //Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().CPI, Is.EqualTo(1.9m));
        }
    }
}
