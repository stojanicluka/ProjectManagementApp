namespace ProjectManagementAPI.Services.Exceptions
{
    public class InvalidEmailFormatException : APIException
    {
        public InvalidEmailFormatException(string message) : base(message) { }

        public override string getType()
        {
            return "InvalidEmailFormatException";
        }
    }
}
