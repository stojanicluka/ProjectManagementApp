namespace ProjectManagementAPI.Services.Exceptions
{
    public class UserNotFoundException : APIException 
    {
        public new String Type { get { return "UserNotFoundException"; } }
        public UserNotFoundException(String message) :base(message) { }
    }
}
