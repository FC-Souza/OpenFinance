using Microsoft.EntityFrameworkCore;
using WebAPI.OpenFinance.Data;
using WebAPI.OpenFinance.Models;

namespace WebAPI.OpenFinance.Validations
{
    //Manage all the methods used to authenticate the client
    public class AuthenticatorHelper
    {
        //Context to access the database
        private OpenFinanceContext _context;

        public AuthenticatorHelper(OpenFinanceContext context)
        {
            _context = context;
        }

        //Get the client with the email
        public async Task<ClientsModel> GetClientWithEmail(string email)
        {
            return await _context.Clients
                .Where(c => c.clientEmail == email)
                .FirstOrDefaultAsync();
        }

        //Get the client_credential with the client_id


        //Check if the email exists in the database


        //Check if the password is correct


    }
}
