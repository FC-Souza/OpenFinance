using WebAPI.OpenFinance.Models;
using WebAPI.OpenFinance.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.OpenFinance.Routes
{
    public static class AuthenticationRoute
    {
        public static void AuthenticationRoutes(this WebApplication app)
        {
            var route = app.MapGroup("authentication");

            //POST /login
            //Will receive a JSON with the email and password
            //Will return a JSON with the clientID and Full Name
            route.MapPost("/login", async (OpenFinanceContext context, Login login) =>
            {

                //Receive the JSON with email and password
                //Get the email and password from the JSON
                var email = login.Email;
                var password = login.Password;

                //Get the client_id with the email
                var client = await context.Clients
                    .Where(c => c.clientEmail == email)
                    .FirstOrDefaultAsync();

                //Check the password received with the password at client_crendential
                var clientCredential = await context.ClientCredentials
                    .Where(c => c.clientID == client.clientID && c.clientPassword == password)
                    .FirstOrDefaultAsync();

                //Return the client_id and client_name
                var loginResponse = new
                {
                    clientID = client.clientID,
                    clientName = client.clientName
                };

                return Results.Ok(loginResponse);


            });

            //POST /Signup
            //Will receive a JSON with the email, password, full name and address
            //Will return a JSON with the clientID and Full Name
            //Password will be stored at client_credential
            route.MapPost("/signup", async (OpenFinanceContext context, Signup signup)
                =>
            {
                //Get the email, password, full name and address from the JSON
                var email = signup.Email;
                var password = signup.Password;
                var name = signup.Name;
                var address = signup.Address;

                //Add the client to the clients table
                var client = new ClientsModel
                {
                    clientName = name,
                    clientEmail = email,
                    clientAddress = address
                };
                context.Clients.Add(client);
                await context.SaveChangesAsync();

                //Get the client_id with the email
                var clientID = await context.Clients
                    .Where(c => c.clientEmail == email)
                    .Select(c => c.clientID)
                    .FirstOrDefaultAsync();

                //Add the client to the client_credential table
                var clientCredential = new ClientCredentialModel
                {
                    clientID = clientID,
                    clientPassword = password
                };
                context.ClientCredentials.Add(clientCredential);
                await context.SaveChangesAsync();

                //Return the client_id and client_name
                var signupResponse = new
                {
                    clientID = clientID,
                    clientName = name
                };

                return Results.Ok(signupResponse);
            });




        }
    }

    //Class to receive the JSON from Login
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    //Class to receive the JSON from Signup
    public class Signup
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
