namespace ProjectManagementAPI.Services.Exceptions
{
    public class DuplicateUsernameException : APIException
    {
        public DuplicateUsernameException(string message) : base(message) { }

        public override string getType()
        {
            return "DuplicateUsernameException";
        }
    }
}
