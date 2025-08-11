namespace ProjectManagementAPI.Services.Exceptions
{
    public class DatabaseException : APIException
    {
        public new String Type { get { return "DatabaseException"; } }
        public DatabaseException(String message) : base(message) { }
    }
}
