namespace ProjectManagementAPI.Services.Exceptions
{
    public class UnauthorizedException : APIException
    {
        public UnauthorizedException(String message) : base(message) { }

        public override String getType()
        {
            return "UnauthorizedException";
        }
    }
}
