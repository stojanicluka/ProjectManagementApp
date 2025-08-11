namespace ProjectManagementAPI.Services.Exceptions
{
    public class ProjectNotFoundException : APIException
    {
        public ProjectNotFoundException(String message) : base(message) { }

        public override String getType()
        {
            return "ProjectNotFoundException";
        }
    }
}
