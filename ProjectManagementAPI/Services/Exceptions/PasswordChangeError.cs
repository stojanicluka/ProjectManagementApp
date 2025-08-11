namespace ProjectManagementAPI.Services.Exceptions
{
    public class PasswordChangeError : APIException
    {
        public PasswordChangeError(string message) : base(message) { }

        public override string getType()
        {
            return "PasswordChangeError";
        }
    }
}
