namespace ProjectManagementAPI.Services.Exceptions
{
    public class APIException : Exception
    {
        public String Type { get { return "NONE"; } }
        public APIException(String message) : base(message) { }
    }
}
