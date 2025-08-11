namespace ProjectManagementAPI.Services.Exceptions
{
    public class DatabaseException : APIException
    {
        public DatabaseException(String message) : base(message) { }

        public override String getType()
        {
            return "DatabaseException";
        }
    }
}
