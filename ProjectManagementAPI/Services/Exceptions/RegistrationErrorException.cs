using Microsoft.Identity.Client;

namespace ProjectManagementAPI.Services.Exceptions
{
    public class RegistrationErrorException : APIException
    {
        public RegistrationErrorException(string message) : base(message) { }

        public override string getType()
        {
            return "RegistrationErrorException";
        }
    }
}
