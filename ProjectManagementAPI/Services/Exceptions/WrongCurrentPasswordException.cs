namespace ProjectManagementAPI.Services.Exceptions
{
    public class WrongCurrentPasswordException : APIException
    {
        public WrongCurrentPasswordException(string message) : base(message) { }

        public override string getType()
        {
            return "WrongCurrentPasswordException";
        }
    }
}
