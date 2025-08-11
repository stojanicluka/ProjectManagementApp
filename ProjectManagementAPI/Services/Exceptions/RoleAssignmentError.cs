namespace ProjectManagementAPI.Services.Exceptions
{
    public class RoleAssignmentError : APIException
    {
        public RoleAssignmentError(string message) : base(message) { }

        public override string getType()
        {
            return "RoleAssignmentError";
        }
    }
}
