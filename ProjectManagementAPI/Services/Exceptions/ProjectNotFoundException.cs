namespace ProjectManagementAPI.Services.Exceptions
{
    public class ProjectNotFoundException : APIException
    {
        public new String Type { get { return "ProjectNotFoundException"; } }
        public ProjectNotFoundException(String message) : base(message) { }
    }
}
