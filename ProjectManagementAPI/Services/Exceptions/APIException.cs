namespace ProjectManagementAPI.Services.Exceptions
{
    public class APIException : Exception
    {
        public APIException(String message) : base(message) { }

        public virtual String getType()
        {
            return "NONE";
        }
    }
}
