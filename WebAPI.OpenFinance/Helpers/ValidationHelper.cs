using System.Text.RegularExpressions;

namespace WebAPI.OpenFinance.Helpers
{
    //Internal, because the class will be used only in this project
    internal static class ValidationHelper
    {
        //Check if the email is valid using REGEX
        internal static bool IsValidEmail(string email)
        {
            //Regex regexPattern for email
            string regexPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

            //Check the REGEX and return the result
            return Regex.IsMatch(email, regexPattern);
        }

        //Check if the password is valid using REGEX
        //8 characters minimum, 1 uppercase, 1 lowercase, 1 number and 1 special character
        internal static bool IsValidPassword(string password)
        {
            //Regex for password
            string regexPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";

            //Check the REGEX and return the result
            return Regex.IsMatch(password, regexPattern);
        }

        //Check if the name is valid using REGEX
        //Only letters and spaces, 3 characters minimum
        internal static bool IsValidName(string name)
        {
            //Regex for name
            string regexPattern = @"^[a-zA-Z\s]{3,}$";

            //Check the REGEX and return the result
            return Regex.IsMatch(name, regexPattern);
        }

        //Check if the address is valid
        //5 characters minimum
        internal static bool IsValidAddress(string address)
        {
            //Check the length
            return address.Length >= 5;
        }

    }
}
