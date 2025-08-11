namespace ProjectManagementAPI.Services.Exceptions
{
    public class UserNotFoundException : APIException 
    {
        public UserNotFoundException(String message) :base(message) { }

        public override String getType()
        {
            return "UserNotFoundException";
        }
    }
}
