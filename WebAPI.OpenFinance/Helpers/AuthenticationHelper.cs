using Microsoft.EntityFrameworkCore;
using WebAPI.OpenFinance.Data;
using WebAPI.OpenFinance.Models;

namespace WebAPI.OpenFinance.Helpers
{
    //Manage all the methods used to authenticate the client
    public static class AuthenticationHelper
    {
        //Context to access the database
        //private OpenFinanceContext _context;

        //public AuthenticatorHelper(OpenFinanceContext context)
        //{
        //    _context = context;
        //}

        //Get the client with the email
        public static async Task<ClientsModel> GetClientByEmail(OpenFinanceContext context, string email)
        {
            var client = await context.Clients
                .Where(c => c.clientEmail == email)
                .FirstOrDefaultAsync();

            return client;
        }


        //Check if the email exists in the database
        public static async Task<bool> CheckEmailExists(OpenFinanceContext context, string email)
        {
            var client = await context.Clients
                .Where(c => c.clientEmail == email)
                .FirstOrDefaultAsync();

            if (client == null)
            {
                return false;
            }

            return true;
        }

        //Check if the password is correct
        public static async Task<bool> CheckPassword(OpenFinanceContext context, int client, string password)
        {
            var credential = await context.ClientCredentials
                .Where(c => c.clientID == client && c.clientPassword == password)
                .FirstOrDefaultAsync();

            //Wrogn password
            if (credential == null)
            {
                //Decrease the remainingLoginAttempts
                await DecreaseRemainingLoginAttempts(context, client);

                return false;
            }

            return true;
        }

        //Register the new client and return the client_id
        public static async Task<int> RegisterClient(OpenFinanceContext context, string clientName, string clientEmail, string clientAddress)
        {
            var newClient = new ClientsModel
            {
                clientName = clientName,
                clientEmail = clientEmail,
                clientAddress = clientAddress
            };

            context.Clients.Add(newClient);
            await context.SaveChangesAsync();

            return newClient.clientID;
        }

        //Register the new client credential
        public static async Task RegisterClientCredential(OpenFinanceContext context, int clientID, string clientPassword)
        {
            var newCredential = new ClientCredentialModel
            {
                clientID = clientID,
                clientPassword = clientPassword
            };

            context.ClientCredentials.Add(newCredential);
            await context.SaveChangesAsync();
        }

        //Get the clientCredential by the client_id
        public static async Task<ClientCredentialModel> GetClientCredentialByClientID(OpenFinanceContext context, int clientID)
        {
            var clientCredential = await context.ClientCredentials
                .Where(c => c.clientID == clientID)
                .FirstOrDefaultAsync();

            return clientCredential;
        }

        //Update the last login of the client
        public static async Task UpdateLastLogin(OpenFinanceContext context, int clientID)
        {
            var clientCredential = await GetClientCredentialByClientID(context, clientID);
            
            clientCredential.lastLogin = DateTime.UtcNow;

            clientCredential.remainingLoginAttempts = 3;

            await context.SaveChangesAsync();
        }

        //Decrease remainingLoginAttempts
        public static async Task DecreaseRemainingLoginAttempts(OpenFinanceContext context, int clientID)
        {
            var clientCredential = await GetClientCredentialByClientID(context, clientID);

            //Decrease the remainingLoginAttempts
            clientCredential.remainingLoginAttempts--;

            //If the remainingLoginAttempts is 0, block the client
            if (clientCredential.remainingLoginAttempts <= 0)
            {
                await BlockClient(context, clientID);
            }

            await context.SaveChangesAsync();
        }

        //Check if the client is blocked
        public static async Task<bool> CheckIfClientIsBlocked(OpenFinanceContext context, int clientID)
        {
            var clientCredential = await GetClientCredentialByClientID(context, clientID);

            if ((clientCredential.isBlocked) && clientCredential.blockedUntil > DateTime.UtcNow)
            {
                return true;
            }

            //Unblock the client
            await UnblockClient(context, clientID);

            return false;
        }

        //Block the client
        public static async Task BlockClient(OpenFinanceContext context, int clientID)
        {
            var clientCredential = await GetClientCredentialByClientID(context, clientID);

            //Blocking
            clientCredential.isBlocked = true;

            //Blocking for 5 minutes
            clientCredential.blockedUntil = DateTime.UtcNow.AddMinutes(5);

            await context.SaveChangesAsync();
        }

        //Unblock the client
        public static async Task UnblockClient(OpenFinanceContext context, int clientID)
        {
            var clientCredential = await GetClientCredentialByClientID(context, clientID);

            //Unblocking
            clientCredential.isBlocked = false;

            //Removing the blockedUntil
            clientCredential.blockedUntil = null;

            await context.SaveChangesAsync();
        }



    }
}
