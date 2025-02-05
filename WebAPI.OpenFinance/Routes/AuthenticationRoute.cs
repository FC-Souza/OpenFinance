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

        
        }
    }

    //Class to receive the JSON from Login
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
