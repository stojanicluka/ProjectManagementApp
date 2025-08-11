namespace ProjectManagementAPI.Services.Exceptions
{
    public class UserNotFoundException : Exception 
    {
        public UserNotFoundException(String message) :base(message) { }
    }
}
