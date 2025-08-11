namespace ProjectManagementAPI.Services.Exceptions
{
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException(String message) : base(message) { }
    }
}
