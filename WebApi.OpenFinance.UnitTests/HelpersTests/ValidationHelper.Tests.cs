using NUnit.Framework;
using WebAPI.OpenFinance.Helpers;
using WebAPI.OpenFinance.Models;
using WebApi.OpenFinance.UnitTests.Helpers;
using System.Threading.Tasks;
using WebAPI.OpenFinance.Responses;

namespace WebApi.OpenFinance.UnitTests.HelpersTests
{
    [TestFixture]
    public class ValidationHelperTests
    {

        [Test]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            //Arrange
            var email = "test@example.com";

            //Act
            var result = ValidationHelper.IsValidEmail(email);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidEmail_InvalidEmail_ReturnsFalse()
        {
            //Arrange
            var email = "invalid-email";

            //Act
            var result = ValidationHelper.IsValidEmail(email);

            //Assert
             Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidPassword_ValidPassword_ReturnsTrue()
        {
            //Arrange
            var password = "Password123!";

            //Act
            var result = ValidationHelper.IsValidPassword(password);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidPassword_InvalidPassword_ReturnsFalse()
        {
            //Arrange
            var password = "password";

            //Act
            var result = ValidationHelper.IsValidPassword(password);

            //Assert
             Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidName_ValidName_ReturnsTrue()
        {
            //Arrange
            var name = "John Doe";

            //Act
            var result = ValidationHelper.IsValidName(name);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidName_InvalidName_ReturnsFalse()
        {
            //Arrange
            var name = "John123";

            //Act
            var result = ValidationHelper.IsValidName(name);

            //Assert
             Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidAddress_ValidAddress_ReturnsTrue()
        {
            //Arrange
            var address = "123 Main St";

            //Act
            var result = ValidationHelper.IsValidAddress(address);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidAddress_InvalidAddress_ReturnsFalse()
        {
            //Arrange
            var address = "123";

            //Act
            var result = ValidationHelper.IsValidAddress(address);

            //Assert
             Assert.That(result, Is.False);
        }

        [Test]
        public async Task ValidateFields_ValidData_ReturnsEmptyString()
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

            var clientProfile = new UpdateClientProfile
            {
                clientID = 1,
                clientName = "John Doe",
                clientEmail = "test@example.com",
                clientAddress = "123 Main St"
            };

            //Act
            var result = await ValidationHelper.ValidateFields(context, clientProfile);

            //Assert
            Assert.That(result, Is.EqualTo(""));
        }
    }
}
