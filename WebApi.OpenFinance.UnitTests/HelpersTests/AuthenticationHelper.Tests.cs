using NUnit.Framework;
using WebAPI.OpenFinance.Helpers;
using WebAPI.OpenFinance.Models;
using WebApi.OpenFinance.UnitTests;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApi.OpenFinance.UnitTests.HelpersTests
{
    [TestFixture]
    public class AuthenticationHelperTests
    {
        [Test]
        public async Task GetClientByEmail_ExistingEmail_ReturnsClient()
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
            var result = await AuthenticationHelper.GetClientByEmail(context, "existing@example.com");

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.clientEmail, Is.EqualTo("existing@example.com"));
        }

        [Test]
        public async Task GetClientByEmail_NonExistingEmail_ReturnsNull()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();

            //Act
            var result = await AuthenticationHelper.GetClientByEmail(context, "nonexisting@example.com");

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CheckEmailExists_ExistingEmail_ReturnsTrue()
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
            var result = await AuthenticationHelper.CheckEmailExists(context, "existing@example.com");

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task CheckEmailExists_NonExistingEmail_ReturnsFalse()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();

            //Act
            var result = await AuthenticationHelper.CheckEmailExists(context, "nonexisting@example.com");

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task CheckPassword_CorrectPassword_ReturnsTrue()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.ClientCredentials.Add(new ClientCredentialModel
            {
                clientID = 1,
                clientPassword = "Password123!"
            });
            await context.SaveChangesAsync();

            //Act
            var result = await AuthenticationHelper.CheckPassword(context, 1, "Password123!");

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task CheckPassword_IncorrectPassword_ReturnsFalse()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.ClientCredentials.Add(new ClientCredentialModel
            {
                clientID = 1,
                clientPassword = "Password123!"
            });
            await context.SaveChangesAsync();

            //Act
            var result = await AuthenticationHelper.CheckPassword(context, 1, "WrongPassword");

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RegisterClient_ValidData_ReturnsClientId()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();

            //Act
            var clientId = await AuthenticationHelper.RegisterClient(context, "John Doe", "new@example.com", "123 Main St");

            //Assert
            var client = await context.Clients.FindAsync(clientId);
            Assert.That(client, Is.Not.Null);
            Assert.That(client.clientEmail, Is.EqualTo("new@example.com"));
        }

        [Test]
        public async Task RegisterClientCredential_ValidData_AddsCredential()
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
            await AuthenticationHelper.RegisterClientCredential(context, 1, "Password123!");

            //Assert
            var credential = await context.ClientCredentials.FirstOrDefaultAsync(c => c.clientID == 1);
            Assert.That(credential, Is.Not.Null);
            Assert.That(credential.clientPassword, Is.EqualTo("Password123!"));
        }

        [Test]
        public async Task GetClientCredentialByClientID_ExistingClientID_ReturnsCredential()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.ClientCredentials.Add(new ClientCredentialModel
            {
                clientID = 1,
                clientPassword = "Password123!"
            });
            await context.SaveChangesAsync();

            //Act
            var result = await AuthenticationHelper.GetClientCredentialByClientID(context, 1);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.clientPassword, Is.EqualTo("Password123!"));
        }

        [Test]
        public async Task GetClientCredentialByClientID_NonExistingClientID_ReturnsNull()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();

            //Act
            var result = await AuthenticationHelper.GetClientCredentialByClientID(context, 999);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateLastLogin_ValidClientID_UpdatesLastLogin()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.ClientCredentials.Add(new ClientCredentialModel
            {
                clientID = 1,
                clientPassword = "Password123!",
                lastLogin = null,
                remainingLoginAttempts = 1
            });
            await context.SaveChangesAsync();

            //Act
            await AuthenticationHelper.UpdateLastLogin(context, 1);

            //Assert
            var credential = await context.ClientCredentials.FirstOrDefaultAsync(c => c.clientID == 1);
            Assert.That(credential.lastLogin, Is.Not.Null);
            Assert.That(credential.remainingLoginAttempts, Is.EqualTo(3));
        }

        [Test]
        public async Task DecreaseRemainingLoginAttempts_ValidClientID_DecreasesAttempts()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.ClientCredentials.Add(new ClientCredentialModel
            {
                clientID = 1,
                clientPassword = "Password123!",
                remainingLoginAttempts = 3
            });
            await context.SaveChangesAsync();

            //Act
            await AuthenticationHelper.DecreaseRemainingLoginAttempts(context, 1);

            //Assert
            var credential = await context.ClientCredentials.FirstOrDefaultAsync(c => c.clientID == 1);
            Assert.That(credential.remainingLoginAttempts, Is.EqualTo(2));
        }

        [Test]
        public async Task CheckIfClientIsBlocked_ClientNotBlocked_ReturnsFalse()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.ClientCredentials.Add(new ClientCredentialModel
            {
                clientID = 1,
                clientPassword = "Password123!",
                isBlocked = false
            });
            await context.SaveChangesAsync();

            //Act
            var result = await AuthenticationHelper.CheckIfClientIsBlocked(context, 1);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task CheckIfClientIsBlocked_ClientBlocked_ReturnsTrue()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.ClientCredentials.Add(new ClientCredentialModel
            {
                clientID = 1,
                clientPassword = "Password123!",
                isBlocked = true,
                blockedUntil = DateTime.UtcNow.AddMinutes(5)
            });
            await context.SaveChangesAsync();

            //Act
            var result = await AuthenticationHelper.CheckIfClientIsBlocked(context, 1);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task BlockClient_ValidClientID_BlocksClient()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.ClientCredentials.Add(new ClientCredentialModel
            {
                clientID = 1,
                clientPassword = "Password123!",
                isBlocked = false
            });
            await context.SaveChangesAsync();

            //Act
            await AuthenticationHelper.BlockClient(context, 1);

            //Assert
            var credential = await context.ClientCredentials.FirstOrDefaultAsync(c => c.clientID == 1);
            Assert.That(credential.isBlocked, Is.True);
            Assert.That(credential.blockedUntil, Is.GreaterThan(DateTime.UtcNow));
        }

        [Test]
        public async Task UnblockClient_ValidClientID_UnblocksClient()
        {
            //Arrange
            using var context = TestDbContextFactory.CreateInMemoryContext();
            context.ClientCredentials.Add(new ClientCredentialModel
            {
                clientID = 1,
                clientPassword = "Password123!",
                isBlocked = true,
                blockedUntil = DateTime.UtcNow.AddMinutes(5)
            });
            await context.SaveChangesAsync();

            //Act
            await AuthenticationHelper.UnblockClient(context, 1);

            //Assert
            var credential = await context.ClientCredentials.FirstOrDefaultAsync(c => c.clientID == 1);
            Assert.That(credential.isBlocked, Is.False);
            Assert.That(credential.blockedUntil, Is.Null);
        }
    }
}
