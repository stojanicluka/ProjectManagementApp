namespace ProjectManagementAPI.Services.Exceptions
{
    public class RoleNotFoundException : APIException 
    {
        public RoleNotFoundException(String message) :base(message) { }

        public override String getType()
        {
            return "RoleNotFoundException";
        }
    }
}
