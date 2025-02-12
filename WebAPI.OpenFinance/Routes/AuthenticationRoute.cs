using WebAPI.OpenFinance.Models;
using WebAPI.OpenFinance.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI.OpenFinance.Helpers;
using Microsoft.AspNetCore.SignalR;
using System.Net.Http.Headers;

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
                //var client = await context.Clients
                //    .Where(c => c.clientEmail == email)
                //    .FirstOrDefaultAsync();
                var client = await AuthenticationHelper.GetClientByEmail(context, email);

                //Check if the cliend is registered
                //if (client == null)
                if (!await AuthenticationHelper.CheckEmailExists(context, email))
                {
                    return Results.BadRequest("Client not found");
                }


                //Check the password received with the password at client_crendential
                //var clientCredential = await context.ClientCredentials
                //    .Where(c => c.clientID == client.clientID && c.clientPassword == password)
                //    .FirstOrDefaultAsync();
                //bool isPasswordCorrect = await AuthenticationHelper.CheckPassword(context, client.clientID, password);

                //if (!isPasswordCorrect)
                if (!await AuthenticationHelper.CheckPassword(context, client.clientID, password))
                {
                    return Results.BadRequest("Incorrect Password");
                }

                //Update the last_login and remaining_login_attempts
                await AuthenticationHelper.UpdateLastLogin(context, client.clientID);

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

                //Input Validations using ValidationHelper
                if (!ValidationHelper.IsValidEmail(email))
                {
                    return Results.BadRequest("Invalid Email");
                }

                if (!ValidationHelper.IsValidPassword(password))
                {
                    return Results.BadRequest("Invalid Password");
                }

                if (!ValidationHelper.IsValidName(name))
                {
                    return Results.BadRequest("Invalid Name");
                }

                if (!ValidationHelper.IsValidAddress(address))
                {
                    return Results.BadRequest("Invalid Address");
                }

                //Checkin if the email is in use. If has existingClient, the email is in use
                //if (existingClient != null)
                if (await AuthenticationHelper.CheckEmailExists(context, email))
                {
                    return Results.BadRequest("Email already in use");
                }

                //Add the NEW client to the clients table after the validations
                //var client = new ClientsModel
                //{
                //    clientName = name,
                //    clientEmail = email,
                //    clientAddress = address
                //};
                //context.Clients.Add(client);
                //await context.SaveChangesAsync();
                var newClientID = await AuthenticationHelper.RegisterClient(context, name, email, address);



                //Get the client_id from the new client added
                //var newClientID = client.clientID;

                ////Add the client to the client_credential table
                //var clientCredential = new ClientCredentialModel
                //{
                //    clientID = newClientID,
                //    clientPassword = password
                //};
                //context.ClientCredentials.Add(clientCredential);
                //await context.SaveChangesAsync();
                await AuthenticationHelper.RegisterClientCredential(context, newClientID, password);

                //Return the client_id and client_name
                var signupResponse = new
                {
                    clientID = newClientID,
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
